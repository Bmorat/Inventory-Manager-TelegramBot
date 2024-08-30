using System.Collections;

namespace Library
{
    /// <summary>
    /// Representa una sección dentro de un almacén.
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Nombre de la sección.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Diccionario que contiene los ítems en stock mediante la clase ítemstock 
        /// </summary>
        public Dictionary<int, StockItem> items { get; set; }

        public string warehouse { get; set; }

       public Section()
       {

       }
       
        /// <summary>
        /// Constructor que inicializa una nueva instancia de la clase <see cref="Section"/> con el nombre especificado.
        /// </summary>
        /// <param name="name">El nombre de la sección.</param>
       
        public Section(string name)
        {
            Name = name;
            items = new Dictionary<int, StockItem>();
        }
    }
}
