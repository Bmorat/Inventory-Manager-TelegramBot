using System;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de añadir un nuevo depósito al sistema.
    /// </summary>
    public class AddWarehouseHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;
        private readonly IAddressFinder finder;
        private readonly TelegramBotClient botClient;

        /// <summary>
        /// Constructor de la clase <see cref="AddWarehouseHandler"/>.
        /// </summary>
        /// <param name="finder">Instancia para la búsqueda de direcciones.</param>
        /// <param name="botClient">Cliente de bot de Telegram para enviar mensajes y ubicaciones.</param>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        /// <exception cref="ArgumentNullException">Lanza una excepción si <paramref name="finder"/> o <paramref name="botClient"/> son nulos.</exception>
        public AddWarehouseHandler(IAddressFinder finder, TelegramBotClient botClient, BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/AddWh" };
            this.finder = finder ?? throw new ArgumentNullException(nameof(finder));
            this.botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        }

        /// <summary>
        /// Maneja el mensaje recibido para agregar un nuevo depósito con nombre y dirección proporcionados.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario, indicando el resultado de la operación.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            string[] partes = Regex.Split(message.Text, @"(?<=/AddWh)\s*([^ ]+)\s+(.*)", RegexOptions.IgnoreCase);

            if (partes.Length < 3)
            {
                response = "Error: debes proporcionar un nombre y una dirección para el almacén.";
                return;
            }

            string name = partes[1].Trim();
            string address = partes[2].Trim();

            name = StringExtensions.Capitalize(name);
            _manager.JsonToList();

            if (_manager.Warehouses.FirstOrDefault(x => x.Name == name) == null)
            {
                Warehouse w = new Warehouse(name);
                AddressData data = new AddressData
                {
                    Address = address,
                    Result = this.finder.GetLocation(address)
                };

                if (data.Result.Found)
                {
                    w.Longitude = data.Result.Longitude;
                    w.Latitude = data.Result.Latitude;

                    botClient.SendLocationAsync(
                        chatId: message.Chat.Id,
                        latitude: data.Result.Latitude,
                        longitude: data.Result.Longitude,
                        cancellationToken: default
                    ).Wait();

                    w.Address = address;
                    _manager.Warehouses.Add(w);
                    _manager.SaveChanges();
                    response = $"Depósito '{w.Name}' agregado exitosamente. Presiona /SudoWMenu para volver al menú principal.";
                }
                else
                {
                    response = "Error: ubicación no encontrada.";
                }
            }
            else
            {
                response = "Error: ya existe un depósito con ese nombre. Presiona /SudoWMenu para volver al menú principal.";
            }
        }

        private class AddressData
        {
            /// <summary>
            /// La dirección proporcionada para el depósito.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// Resultados de la búsqueda de la ubicación para la dirección.
            /// </summary>
            public IAddressResult Result { get; set; }
        }
    }
}
