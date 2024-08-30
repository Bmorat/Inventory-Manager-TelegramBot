namespace Library
{
    /// <summary>
    /// Clase que representa a un usuario en el sistema.
    /// </summary>
    public class User : Usuarios, IUser
    {
        /// <summary>
        /// Constructor por defecto de la clase User para archivo Json
        /// </summary>
        public User() { }

        /// <summary>
        /// Constructor de la clase User que recibe el nombre como parámetro.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        public User(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Método para buscar un producto.
        /// </summary>
        public void SearchProduct()
        {
            WarehouseManagerUsers manager = new WarehouseManagerUsers();
            manager.SeachArticuloID();
        }
    }
}
