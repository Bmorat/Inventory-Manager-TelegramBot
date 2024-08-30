namespace Library
{
    public class WarehouseManagerUsers()
    {
        /// <summary>
        /// Utilizamos el patron Singleton para que siempre se la misma instancia de la lista y cuando se refieran otras clases a la lista siempre sea la misma
        /// </sumary>
        WarehouseManager manager = Singleton<WarehouseManager>.Instance;
        
        /// <summary>
        /// Creamos un menu que sea diferente para lo que le conviene a los usuarios
        /// </sumary>
        public void UsersWarehouseMenu()
        {
            Console.WriteLine("Que desea realizar en la Sección Almacenes :\n1_Mostrar Depositos\n2_Agregar Articulo\n3_Buscar Producto por ID\n4_Buscar producto código, descripción y categoría");
            int answer = Convert.ToInt32(Console.ReadLine());
            if (answer == 1)
            {
                ShowWareHouses();
                UsersWarehouseMenu();
            }
            else if (answer == 2)
            {
                AddArticulo();
                UsersWarehouseMenu();
            }
            else if (answer == 3)
            {
                SeachArticuloID();
                UsersWarehouseMenu();                            
            }
            else if (answer == 4)
            {
                SeachIDDescriptionCategory();
                UsersWarehouseMenu();
            }
            else
            {
                Console.WriteLine("Error ");
                UsersWarehouseMenu();
            }
        }



        /// <summary>
        /// Este metodo muestra todos los depositos existentes
        /// </summary>
        void ShowWareHouses()
        {
            foreach(Warehouse w in manager.Warehouses)
            {
                Console.WriteLine($"{w.Name}");
            }
        }
        /// <summary>
        /// ES una forma de busqueda en caso de que el usuario solo conozca el producto por su codigo (id)
        /// </summary>
        public void SeachArticuloID()
        {
            Console.WriteLine("Ingrese el ID del articulo para ver el Stock: ");
            string productId = Console.ReadLine();
            foreach (Warehouse w in manager.Warehouses)
            {
                foreach (Section secc in w.Sections)
                {
                    Console.WriteLine($"Deposito: {w.Name}");
                    foreach (var dic in secc.items)
                    {
                        Console.WriteLine($"Sector: {secc.Name}\nId: {dic.Value.Id} - Nombre: {dic.Value.Item.Name} - Cantindad: {dic.Value.Quantity}");
                    }
                }
            }    
        }
        /// <summary>
        /// Pregunta el articulo que se quiere agregar y se lo agrega al deposito.
        /// </summary>
        void AddArticulo()
        {
            Console.WriteLine("ingrese el nombre del articulo para agregar: ");
            string name = StringExtensions.Capitalize(Console.ReadLine());
            Warehouse deposito = new Warehouse(name);  
            deposito.AgregarItem();
        }
        /// <summary>
        /// Permite buscar un articulo por su Id, su descripcion y su categoria
        /// </summary>
        void SeachIDDescriptionCategory()
        {
            Console.WriteLine("Buscar producto por código, descripción y categoría");
            Console.WriteLine("Código: ");
            int code = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Descripción: ");
            string description = StringExtensions.Capitalize(Console.ReadLine());
            Console.WriteLine("Categoría: ");
            string category = StringExtensions.Capitalize(Console.ReadLine());
            bool productFound = false;
            foreach (Warehouse w in manager.Warehouses)
            {
                foreach (Section secc in w.Sections)
                {
                    foreach (var item in secc.items.Values)
                    {
                        if (item.Id == code && item.Item.Name.ToLower() == description.ToLower() && secc.Name.ToLower() == category.ToLower())
                        {
                            Console.WriteLine($"Sector: {secc.Name}");
                            Console.WriteLine($"Id: {item.Id} - Nombre: {item.Item.Name} - Cantidad: {item.Quantity}");
                            productFound = true;
                        }
                    }
                }
            }
            if (!productFound)
            {
                Console.WriteLine("Producto no encontrado.");
            }
        }
    }
}

