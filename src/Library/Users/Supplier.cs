namespace Library
{
    /// <summary>
    /// Clase que representa a un proveedor en el sistema.
    /// </summary>
    public class Supplier : Usuarios, IUser
    {


        /// <summary>
        /// Método para buscar un producto.
        /// </summary>
        public void SearchProduct()
        {
            WarehouseManagerSupplier user = new WarehouseManagerSupplier();
            user.SeachArticuloID();
        }

        /// <summary>
        /// Constructor por defecto de la clase Supplier para utilizar el archivo Json
        /// </summary>
        public Supplier() { }

        /// <summary>
        /// Constructor de la clase Supplier que recibe el nombre como parámetro.
        /// </summary>
        /// <param name="name">Nombre del proveedor.</param>
        public Supplier(string name)
        {
            this.Name = name;
        }
    }
}