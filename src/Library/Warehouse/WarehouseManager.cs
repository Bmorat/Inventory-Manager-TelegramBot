
using System.Text.Json;

namespace Library
{
        /// <summary>
        /// El propósito de esta clase es "quitar" al admin la responsabilidad de administrar los depósitos, si bien por UserStory
        /// el admin es el encargado de esto, para evitar el acoplamiento se crea un administrador de depósitos que maneja el admin.
        /// La intención con el bot es utilizarla con singleton para que sea una única instancia y todos los usuarios manejen la misma lista de depósitos ubicada en warehouses.
        /// Esta clase cumple con SRP porque su responsabilidad es manjear los depósitos y lo que ocurre con los items que tiene dentro
        /// A su vez, cumple con el patrón Expert porque es esta clase conoce las ventas porque sabe que items ingresan y salen de los depositos.
        /// </summary>
    
    public class WarehouseManager
    {
        /// <summary>
        /// Creamos una lista de depositos para tener todos los depositos existentes
        /// </summary>
        public List<Warehouse> Warehouses = new List<Warehouse>();
        /// <summary>
        /// Creamos una lista para poder visualizar todas las ventas efectuadas
        /// </summary> 
        
        string filePath = @"../../docs/Warehouses.json";
        string filePathSells = @"../../docs/Sells.json";
        private string jsonStr;
        private string jsonStrSells;
        public List<Sells> sells = new List<Sells>();

        public Warehouse AuxiliarWarehouse {get; set;}
        
        public void WarehouseMenu()
        {
            Console.WriteLine("Que desea realizar en la Sección Almacenes : \n1_Mostrar depositos\n2_Agregar deposito\n3_Agregar item\n4_Remover item\n5_Ver inventarios\n6_Ver ventas\n7_ Volver al menu principal");
            int answer = Convert.ToInt32(Console.ReadLine());
            if (answer == 1)
            {
                ShowWarehouses();
                WarehouseMenu();
            }
            else if (answer == 2)
            {
                AddWarehouses();
                WarehouseMenu();
            }
            else if (answer == 3)
            {
                AddItem();
                WarehouseMenu();
            }
            else if (answer == 4)
            {
                RemoveItem();
                WarehouseMenu();
            }
            else if(answer == 5)
            {  
                ShowInventory();
                WarehouseMenu();                
            }
            else if (answer == 6)
            {
                SellViewer();
                WarehouseMenu();
            }
            else if (answer == 7)
            {
              AppRunner.menuAdmin();
            }
            else
            {
                Console.WriteLine("Error ");
                WarehouseMenu();
            }
        }



        /// <summary>
        /// Muestra cada uno de los depositos existentes a través de iterar con un foreach
        /// </summary>
        void ShowWarehouses()
        {
            Console.WriteLine("Sus depositos son:");
            int n = 1;
            foreach(Warehouse w in Warehouses)
            {
                Console.WriteLine($"{n}. {w.Name}\n");
                n++;
            }
        }
        /// <summary>
        /// Agrega depositos a la lista Warehouses en caso de que su nombre no este registrado
        /// </summary>
        public void AddWarehouses()
        {
            Console.WriteLine("ingrese el nombre del deposito");
            string name = StringExtensions.Capitalize(Console.ReadLine());
            Warehouse WarehouseFound = Warehouses.FirstOrDefault(x => x.Name == name);
            if (WarehouseFound == null)
            {
                Warehouse deposito = new Warehouse(name);
                Warehouses.Add(deposito);
            }
            else 
            {
                Console.WriteLine("Error, ya existe un deposito con ese nombre");
            }
        }
        /// <summary>
        /// Pregunta en que deposito quiere agregar el item y comprueba que ese deposito exista
        /// </summary>
        public void AddItem()
        {
            Console.WriteLine("ingrese el nombre del deposito en el que desea agregar items");
            string name = StringExtensions.Capitalize(Console.ReadLine());
            Warehouse WarehouseFound = Warehouses.Find(x => x.Name == name);
            if (WarehouseFound !=null)
            {
                WarehouseFound.AgregarItem();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
        /// <summary>
        /// Permite realizar las ventas preguntando en que deposito quiere vender y comprobando que ese deposito exista. 
        /// A su vez utiliza el patron Observer para poder actualizar los inventarios cada vez que haya un retiro.
        /// </summary>
        void RemoveItem()
        {
            Console.WriteLine("ingrese el nombre del deposito en el que desea vender items (remover)");
            string name=StringExtensions.Capitalize(Console.ReadLine());
            Warehouse WarehouseFound = Warehouses.Find(x => x.Name == name);
            InventoryObserver Observer = new InventoryObserver();
            WarehouseFound.Subscribe(Observer);
            if (WarehouseFound !=null)
            {
                
                Sells sells = WarehouseFound.SellItem();
                this.sells.Add(sells);

            }
            else
            {
                Console.WriteLine("Error");
            }

        }
        /// <summary>
        /// Comprueba que exista el deposito al que se quiere acceder y muestra el inventario de este.
        /// </summary>
        void ShowInventory()
        {
            Console.WriteLine("ingrese el nombre del deposito en el que desea ver el inventario");
            string name = StringExtensions.Capitalize(Console.ReadLine());
            Warehouse WarehouseFound = Warehouses.Find(x => x.Name == name);
            if (WarehouseFound !=null)
            {
                WarehouseFound.ShowInventory();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        /// <summary>
        /// Permite ver todas las ventas realizadas hasta el momento, recorre la lista sells y muestra cada sell 
        /// </summary>      
        public void SellViewer()
        {
            Console.WriteLine("Las ventas del día son:");
            foreach (var sell in sells)
            {
                Console.WriteLine($"{sell.Product} --> {sell.Quantity}");
            }
        }

        public void JsonToList()
        {
            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
                
                
                Warehouses = JsonSerializer.Deserialize<List<Warehouse>>(jsonStr) ?? new List<Warehouse>();
                          // Carga las secciones para cada almacén
                foreach (var warehouse in Warehouses)
                {
                    warehouse.LoadSections();
                }
                
            }

        }
        public void SaveChanges()
        {
            jsonStr = JsonSerializer.Serialize(Warehouses, new JsonSerializerOptions { WriteIndented = true });
            
            File.WriteAllText(filePath, jsonStr);
            
        }

        public void SellList()
        {
            jsonStrSells = File.ReadAllText(filePathSells);
            sells = JsonSerializer.Deserialize<List<Sells>>(jsonStrSells) ?? new List<Sells>();
        }
        public void sellsave()
        {
            jsonStrSells = JsonSerializer.Serialize(sells, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePathSells, jsonStrSells);
        }
    }
}
