using System;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el inventario de un depósito.
    /// </summary>
    public class ShowInventoryHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;  // Instancia de la clase WarehouseManager
        
        /// <summary>
        /// Constructor de la clase <see cref="ShowInventoryHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public ShowInventoryHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/ShowInventory" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y muestra el inventario del depósito especificado.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Admin admin = new Admin();
            admin.UserManager.JsonToList();
            
            // Validación de que UserManager y sus listas no sean nulas
            if (admin.UserManager == null)
            {
                response = "Error: No se pudo acceder al UserManager.";
                return;
            }

            if (admin.UserManager.RegisteredUsers == null || admin.UserManager.RegisteredSuppliers == null)
            {
                response = "Error: No se han registrado usuarios o proveedores.";
                return;
            }

            // Verificar si el usuario o proveedor está registrado y logueado
            Supplier suppFound = admin.UserManager.RegisteredSuppliers.Find(x => x.ChatId == message.From.Id);
            User userFound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

            if ((userFound != null && userFound.logged) || (suppFound != null && suppFound.logged))
            {
                // Obtener el nombre del depósito del mensaje
                string[] partes = message.Text.Split(new string[] { "/ShowInventory" }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 0)
                {
                    response = "Por favor, especifica el nombre del depósito después del comando /ShowInventory.";
                    return;
                }
                string name = partes[0].Trim();
                name = StringExtensions.Capitalize(name);
                _manager.JsonToList();
                Warehouse warehouse = _manager.Warehouses.Find(x => x.Name == name);

                if (warehouse != null)
                {
                    warehouse.JsonToList();  // Cargar la lista del JSON del depósito específico
                    // Filtrar las secciones que pertenecen al depósito especificado
                    var sectionsInWarehouse = warehouse.Sections.Where(s => s.warehouse.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

                    if (sectionsInWarehouse.Any())
                    {
                        // Enviar la información del inventario del depósito
                        TelegramBot.SendTextMessageAsync($"Inventario de {warehouse.Name}",message.From.Id);
                        Thread.Sleep(50); // Pausa breve para asegurar la correcta entrega de mensajes
                        foreach (Section section in sectionsInWarehouse)
                        {
                            TelegramBot.SendTextMessageAsync($"Sector: {section.Name}",message.From.Id);
                            foreach (var itemEntry in section.items)
                            {
                                var stockItem = itemEntry.Value;
                                TelegramBot.SendTextMessageAsync($"Id {stockItem.Id} - Nombre: {stockItem.Item.Name} - Cantidad: {stockItem.Quantity}",message.From.Id);
                            }
                        }
                        Thread.Sleep(50);
                        response = "Inventario mostrado exitosamente.\nEscriba /SudoWMenu para volver al menú de depósitos.";
                    }
                    else
                    {
                        response = "No se encontraron secciones en el depósito especificado.";
                    }
                }
                else
                {
                    response = "Error, no se encontró el depósito. Presiona /SudoWMenu para volver al menú principal.";
                }
            }
            else
            {
                response = "No puedo ejecutar este comando porque no estás logueado como usuario, proveedor o administrador. Por favor, escribe /LoginSession para iniciar sesión.";
            }
        }
    }
}
