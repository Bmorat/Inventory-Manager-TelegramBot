using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de preparar la solicitud para mostrar el inventario de un depósito.
    /// </summary>
    public class PreShowInventoryHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;  // Instancia de la clase WarehouseManager

        /// <summary>
        /// Constructor de la clase <see cref="PreShowInventoryHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public PreShowInventoryHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoShowInventory" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y solicita al usuario el nombre del depósito para mostrar su inventario.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Admin admin = new Admin();
            admin.UserManager.JsonToList();

            Supplier suppfound = admin.UserManager.RegisteredSuppliers.Find(x => x.ChatId == message.From.Id);
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

            if (userfound != null)
            {
                if (userfound.logged)
                {
                    response = "Ingrese el nombre del depósito después de escribir /ShowInventory en el que desea ver el inventario:\nEjemplo: /ShowInventory Central";
                }
                else
                {
                    response = "No puedo ejecutar este comando porque no estás loggeado como user, admin o proveedor. Por favor, inicia sesión a través de /LoginSession.";
                }
            }
            else if (suppfound != null)
            {
                if (suppfound.logged)
                {
                    response = "Ingrese el nombre del depósito después de escribir /ShowInventory en el que desea ver el inventario:\nEjemplo: /ShowInventory Central";
                }
                else
                {
                    response = "No puedo ejecutar este comando porque no estás loggeado como user, admin o proveedor. Por favor, inicia sesión a través de /LoginSession.";
                }
            }
            else
            {
                response = "Error: usuario no encontrado.";
            }
        }
    }
}
