namespace Library
{
    /// <summary>
    /// Representa un elemento en el stock del almacen
    /// </summary>
    public class StockItem
    {
        /// <summary>
        /// Obtiene o establece el codigo del producto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el artículo asociado al elemento en el stock.
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad del elemento en el stock.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase StockItem con el identificador, artículo y cantidad especificados.
        /// </summary>
        /// <param name="id">El codigo identificador del elemento en el stock.</param>
        /// <param name="item">El artículo asociado al elemento en el stock.</param>
        /// <param name="quantity">La cantidad del elemento en el stock.</param>
        public StockItem(int id, Item item, int quantity)
        {
            Id = id;
            Item = item;
            Quantity = quantity;
        }

        public StockItem()
        {
            
        }
    }
}
