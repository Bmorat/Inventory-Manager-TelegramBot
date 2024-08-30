using System.Diagnostics.Contracts;
using System.Text.Json;

namespace Library
{
    /// <summary>
    /// Representa un almacén que implementa el patrón de diseño Observer para que podamos saber sobre los movimientos en stock(ventas)
    /// Es un Creator de Item y Section dado que los items tienen una categoría que corresponde a una sección dentro del depósito.
    /// </summary>
    public class Warehouse : IObservable
    {
        /// <summary>
        /// Nombre del almacén.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dirección del almacén.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Latitud de la ubicación del almacén.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitud de la ubicación del almacén.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Lista de secciones dentro del almacén.
        /// </summary>
        public List<Section> Sections = new List<Section>();

        /// <summary>
        /// Lista de observadores suscritos al almacén.
        /// </summary>
        protected List<IObserver> observers = new List<IObserver>();

        /// <summary>
        /// Objeto de la clase ventas para registrar los ítems vendidos
        /// </summary>
        string filePath = @"../../docs/Sections.json";

        private string jsonStr;

        public Sells Vendido;

        /// <summary>
        /// Suscribe un observador al almacén.
        /// </summary>
        /// <param name="observer">El observador que se va a suscribir.</param>
        public void Subscribe(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <summary>
        /// Notifica a todos los observadores suscritos de un cambio.
        /// </summary>
        public void NotifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.Update();
            }
        }

        /// <summary>
        /// Agrega una nueva sección al almacén.
        /// </summary>
        /// <param name="section">La sección que se va a agregar.</param>
        public void AddSection(Section section)
        {
            Contract.Requires(section != null, "La sección no puede ser nula.");
            Sections.Add(section);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Warehouse"/> con el nombre especificado.
        /// </summary>
        /// <param name="name">El nombre del almacén.</param>
        public Warehouse(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name), "El nombre del almacén no puede ser nulo o vacío.");
            Name = name;
        }

        /// <summary>
        /// Agrega un nuevo ítem a una sección del almacén.
        /// </summary>
        public void AgregarItem()
        {
            Console.WriteLine("Agrega el Nombre del producto");
            string ProductName = StringExtensions.Capitalize(Console.ReadLine());
            Item item = new Item(ProductName);
            Console.WriteLine("Agrega la categoría del producto");
            string category = StringExtensions.Capitalize(Console.ReadLine());
            item.Category = category;
            Section section = Sections.FirstOrDefault(s => s.Name == category);
            if (section == null)
            {
                section = new Section(category);
                Sections.Add(section);
            }
            int IdProduct = section.items.Count();
            Console.WriteLine("Agrega la cantidad del producto");
            int ProductQuantity = Convert.ToInt32(Console.ReadLine());
            if (!section.items.ContainsKey(IdProduct))
            {
                section.items[IdProduct] = new StockItem(IdProduct, item, ProductQuantity);
            }
            else
            {
                section.items[IdProduct].Quantity += ProductQuantity;
            }
        }

        /// <summary>
        /// Remueve un ítem de una sección del almacén.
        /// </summary>
        public void RemoverItem()
        {
            Console.WriteLine("Escribe la sección del producto a remover");
            string secc = StringExtensions.Capitalize(Console.ReadLine());
            Section section = Sections.Find(x => x.Name == secc);
            if (section != null)
            {
                Console.WriteLine("Coloca el id del Producto: ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Coloca la Cantidad que desea eliminar: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                if (section.items.ContainsKey(id))
                {
                    section.items[id].Quantity -= quantity;
                    if (section.items[id].Quantity <= 0)
                    {
                        section.items.Remove(id);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: sección no encontrada");
            }
        }

        /// <summary>
        /// Muestra el inventario del almacén.
        /// </summary>
        public void ShowInventory()
        {
            foreach (Section secc in Sections)
            {
                Console.WriteLine($"Sector : {secc.Name}");
                foreach (var dic in secc.items)
                {
                    Console.WriteLine($"Id {dic.Value.Id} - Nombre: {dic.Value.Item.Name} - Cantidad: {dic.Value.Quantity}");
                }
            }
        }

        /// <summary>
        /// Vende un ítem de una sección del almacén.
        /// </summary>
        /// <returns>Retorna un objeto <see cref="Sells"/> que representa la venta realizada.</returns>
        public Sells SellItem()
        {
            Console.WriteLine("Escribe la sección del producto a vender");
            string secc = StringExtensions.Capitalize(Console.ReadLine());
            Section section = Sections.Find(x => x.Name == secc);
            if (section != null)
            {
                Console.WriteLine("Coloca el id del Producto: ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Coloca la Cantidad que desea vender: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                if (section.items.ContainsKey(id))
                {
                    if (section.items[id].Quantity >= quantity)
                    {
                        section.items[id].Quantity -= quantity;
                        Sells Venta = new Sells(section.items[id].Item.Name, section.items[id].Item.Category, quantity);
                        Vendido = Venta;
                        NotifyObservers();//notifica al observador
                    }
                    else
                    {
                        Console.WriteLine("Error: cantidad excedida");
                        Vendido = Sells.InvalidSells;
                    }
                }
                else
                {
                    Console.WriteLine("Error: El producto con el id especificado no existe en la sección.");
                    Vendido = Sells.InvalidSells;
                }
            }
            else
            {
                Console.WriteLine("Error: sección no encontrada");
                Vendido = Sells.InvalidSells;
            }
            return Vendido;
        }
        
        public List<StockItem> MissingStock()
        {
            List<StockItem> missingItems = new List<StockItem>();

            foreach (var section in Sections)
            {
                foreach (var stockItem in section.items.Values)
                {
                    if (stockItem.Quantity == 0)
                    {
                        missingItems.Add(stockItem);
                    }
                }
            }

            return missingItems;
        }  

        public void JsonToList()
        {
            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
                
                
                Sections = JsonSerializer.Deserialize<List<Section>>(jsonStr) ?? new List<Section>();
                
            }

        }
        public void SaveChanges()
        {
            // Leer secciones existentes del archivo
            List<Section> existingSections = new List<Section>();

            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
                existingSections = JsonSerializer.Deserialize<List<Section>>(jsonStr) ?? new List<Section>();
            }

            // Combinar secciones existentes con las nuevas sin duplicar
            foreach (Section section in Sections)
            {
                Section existingSection = existingSections.FirstOrDefault(x => x.Name == section.Name && x.warehouse == section.warehouse);
                if (existingSection == null)
                {
                    existingSections.Add(section);
                }
                else
                {
                    // Actualizar la sección existente
                    int index = existingSections.FindIndex(x => x.Name == section.Name && x.warehouse == section.warehouse);
                    if (index != -1)
                    {
                        existingSections[index] = section;
                    }
                }
            }

            // Serializar y guardar la lista combinada en el archivo
            jsonStr = JsonSerializer.Serialize(existingSections, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonStr);
        }

        public void LoadSections()
    {
        if (File.Exists(filePath))
        {
            string jsonStr = File.ReadAllText(filePath);
            var allSections = JsonSerializer.Deserialize<List<Section>>(jsonStr) ?? new List<Section>();

            // Filtra las secciones para este almacén específico
            Sections = allSections.Where(section => section.warehouse == this.Name).ToList();
        }
    }

}
}

