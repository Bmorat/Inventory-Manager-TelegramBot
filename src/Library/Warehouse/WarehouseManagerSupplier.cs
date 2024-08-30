namespace Library
{
    public class WarehouseManagerSupplier
    {
        /// <summary>
        /// Utilizamos el patron Singleton para que siempre se la misma instancia de la lista y cuando se refieran otras clases a la lista siempre sea la misma
        /// </sumary>
        WarehouseManager manager = Singleton<WarehouseManager>.Instance;    
        /// <summary>
        /// Creamos un menu para los proveedores que tienen distintas funciones que los usuarios/admin
        /// </summary>
        public void SupplierWarehouseMenu()
        {
            Console.WriteLine("Que desea realizar en la Sección Almacenes : \n1_Mostrar Depositos\n2_Buscar el stock de un articulo\n3_Buscar el stock de un articulo por ID");
            int answer = Convert.ToInt32(Console.ReadLine());
            if (answer == 1)
            {
                ShowWarehouse();
                SupplierWarehouseMenu();
            }
            else if (answer == 2)
            {                
                SeachArticulo();
                SupplierWarehouseMenu();
            }
            else if (answer == 3)
            {
                SeachArticuloID();
                SupplierWarehouseMenu();
            }
            
            else
            {
                Console.WriteLine("Error ");
                SupplierWarehouseMenu();
            }
        } 


        /// <summary>
        /// Este metodo muestra todos los depositos existentes
        /// </summary>
        void ShowWarehouse()
        {
            foreach(Warehouse w in manager.Warehouses)
            {
                Console.WriteLine($"{w.Name}");
            }
        }
        /// <summary>
        /// Esta forma de busqueda permite que el proveedor pueda encontrar el Articulo por su nombre
        /// </summary>
        public void SeachArticulo()
        {
            Console.WriteLine("Ingrese el nombre del articulo para ver el Stock: ");
            string name = StringExtensions.Capitalize(Console.ReadLine());
            foreach (Warehouse w in manager.Warehouses)
            {
                foreach (Section section in w.Sections)
                {
                    if(name == section.Name)
                    {
                        Console.WriteLine($"Articulo: {section.Name}, Cantidad: {section.items.Count()}, Sección: {section.Name}, Almacén: {w.Name}");
                    }
                }
            }
        }
        /// <summary>
        /// ES una forma de busqueda en caso de que el proveedor solo conozca el producto por su codigo (id)
        /// </summary>
        public void SeachArticuloID()
        {
            Console.WriteLine("Ingrese el ID del articulo para ver el Stock: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int productId))
            {
                bool productFound = false;
                foreach (Warehouse w in manager.Warehouses)
                {
                    foreach (Section secc in w.Sections)
                    {
                        if (secc.items.ContainsKey(productId))
                        {
                            StockItem stockItem = secc.items[productId];
                            Console.WriteLine($"Sector: {secc.Name}");
                            Console.WriteLine($"Id: {stockItem.Id} - Nombre: {stockItem.Item.Name} - Cantidad: {stockItem.Quantity}");
                            productFound = true;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("ID de producto no válido.");
            }
        }
    }
}

