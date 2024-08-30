namespace Library
{
    /// <summary>
    /// Items para interactuar con stocks almacenes etc
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Nombre del item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Categoria del item
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// inicializa una instancia de item <see cref="Item"/> clase con nombre especifico.
        /// </summary>
        /// <param name="name">el nombre del item.</param>
        public Item(string name)
        {
            Name = name;
        }
    }
}
