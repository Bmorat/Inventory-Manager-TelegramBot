using System;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de buscar un ítem por ID y determinar el almacén más cercano que contiene el ítem.
    /// </summary>
    public class SearchItemIdHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance; // Instancia del gestor de almacenes
        private readonly LocationApiClient client; // Cliente para consultar la distancia
        public StockItem item; // Ítem encontrado

        /// <summary>
        /// Constructor de la clase <see cref="SearchItemIdHandler"/>.
        /// </summary>
        /// <param name="Client">Cliente de la API de ubicación para calcular distancias.</param>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public SearchItemIdHandler(LocationApiClient Client, BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SearchID" };
            client = Client;
        }

        /// <summary>
        /// Maneja el mensaje recibido para buscar un ítem por ID y determinar el almacén más cercano que contiene el ítem.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Admin admin = new Admin();

            string[] parts = message.Text.Split(' '); // Divide el mensaje en partes
            DistanceCalculator calculator = new DistanceCalculator(client); // Calculadora de distancias
            Location location1 = new Location(); // Ubicación del usuario
            Location location2 = new Location(); // Ubicación del almacén

            User FoundUser = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            _manager.JsonToList();
            // Asigna la ubicación del usuario si está registrado
            if (FoundUser != null)
            {
                location1.Latitude = FoundUser.Latitude;
                location1.Longitude = FoundUser.Longitude;
            }

            Distance distanciamin = new Distance { TravelDistance = int.MaxValue }; // Distancia mínima inicial
            string WarehouseCercano = ""; // Nombre del almacén más cercano
            int id = int.MinValue;

            // Verifica si el mensaje tiene el formato correcto
            if (parts.Length == 2 && int.TryParse(parts[1], out int itemId))
            {
                bool productFound = false;
                response = "";
                id = itemId;

                var foundItems = new StringBuilder();
                var processedItems = new HashSet<string>(); // Para rastrear ítems procesados y evitar duplicados

                // Recorre todos los almacenes y secciones para buscar el ítem
                foreach (var warehouse in _manager.Warehouses)
                {
                    
                    foreach (var section in warehouse.Sections)
                    {
                        if (section.items.TryGetValue(itemId, out var stockItem))
                        {
                            // Usar un identificador único para cada ítem en función del almacén y la sección
                            string uniqueIdentifier = $"{warehouse.Name}-{section.Name}-{stockItem.Id}";

                            // Verificar si ya hemos procesado este ítem
                            if (processedItems.Contains(uniqueIdentifier))
                            {
                                continue; // Si ya fue procesado, omitir
                            }

                            processedItems.Add(uniqueIdentifier); // Añadir el ítem a la lista de procesados

                            // Calcula la distancia entre el usuario y el almacén
                            location2.Longitude = warehouse.Longitude;
                            location2.Latitude = warehouse.Latitude;
                            Distance distancia = client.GetDistance(location1, location2);

                            // Actualiza la distancia mínima y el almacén más cercano
                            if (distancia.TravelDistance < distanciamin.TravelDistance)
                            {
                                distanciamin.TravelDistance = distancia.TravelDistance;
                                item = new StockItem(stockItem.Id, stockItem.Item, stockItem.Quantity);
                                WarehouseCercano = warehouse.Name;
                            }

                            // Agrega información del ítem encontrado a la lista
                            foundItems.AppendLine($"Almacén: {warehouse.Name}\nSección: {section.Name}\nID: {stockItem.Id} - Nombre: {stockItem.Item.Name} - Cantidad: {stockItem.Quantity}\n");
                            productFound = true;
                        }
                    }
                }

                // Responde con la ubicación del ítem más cercano o un mensaje de error
                if (!productFound)
                {
                    response = "Artículo no encontrado.";
                }
                else
                {
                    TelegramBot.SendTextMessageAsync(foundItems.ToString(), message.From.Id).Wait();
                    response = $"Los ítems con ID {id} más cercanos están en el depósito '{WarehouseCercano}' a una distancia de {distanciamin.TravelDistance} km";
                }
            }
            else
            {
                response = "Formato incorrecto. Use /SearchID [ID]. Ejemplo: /SearchID 15";
            }
        }
    }
}
