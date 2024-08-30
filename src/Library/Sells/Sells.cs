namespace Library
{
    /// <summary>
    /// Representa una venta en el elmacen
    /// </summary>
    public class    Sells
    {
        /// <summary>
        /// Obtiene o establece el producto vendido.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Obtiene o establece la categoría del producto.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad vendida.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Representa una venta inválida.
        /// </summary>
        public static readonly Sells InvalidSells = new Sells("Venta inválida", "Venta inválida", 0);

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Sells"/> con el producto, categoría y cantidad especificados.
        /// </summary>
        /// <param name="p">El nombre del producto.</param>
        /// <param name="c">La categoría del producto.</param>
        /// <param name="q">La cantidad vendida.</param>
       
       public Sells()
       {
        
       }
        public Sells(string p, string c, int q)
        {
            Product = p;
            Category = c;
            Quantity = q;
        }
    }
}
