namespace Library
{
    /// <summary>
    /// Representa un administrador que hereda de Usuarios e implementa la interfaz IUser.
    /// </summary>
    public class Admin : Usuarios, IUser 
    {
        /// <summary>
        /// Instancia única de la clase que gestiona las listas de usuarios. utilización del patrón Singleton
        /// </summary>
        public UserManager UserManager = Singleton<UserManager>.Instance;

        /// <summary>
        /// Agrega un nuevo usuario a la lista de usuarios registrados.
        /// </summary>
        /// <param name="user">El usuario que se va a agregar.</param>
        public void AddUser(User user)
        {
            UserManager.JsonToList();
            UserManager.RegisteredUsers.Add(user);
            user.id = UserManager.RegisteredUsers.Count; // Asigna un id basado en el número de usuarios en la lista
            user.logged = true; // Marca el usuario como loggeado
            UserManager.SaveChanges();
        }

        /// <summary>
        /// Agrega un nuevo proveedor a la lista de proveedores registrados.
        /// </summary>
        /// <param name="supplier">El proveedor que se va a agregar.</param>
        public void AddSupplier(Supplier supplier)
        {
            UserManager.RegisteredSuppliers.Add(supplier);
            supplier.id = UserManager.RegisteredSuppliers.Count; // Asigna un id basado en el número de proveedores en la lista
            supplier.logged = true; // Marca el proveedor como loggeado
            UserManager.SaveChanges();
        }

        /// <summary>
        /// Llama al menú de gestión de almacenes.
        /// </summary>
        public void NewWarehouse()
        {
            WarehouseManager manager = Singleton<WarehouseManager>.Instance;
            manager.WarehouseMenu();
        }



        /// <summary>
        /// Busca un producto por su código.
        /// </summary>
        public void SearchProduct()
        {
            WarehouseManagerUsers user = new WarehouseManagerUsers();
            user.SeachArticuloID();
        }


        /// <summary>
        /// Muestra las ventas.
        /// </summary>
        public void ViewSales()
        {
            WarehouseManager manager = Singleton<WarehouseManager>.Instance;
            manager.SellViewer();
        }
    }
}