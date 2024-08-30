using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar las ventas del día.
    /// </summary>
    public class SellViewerHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance; // Instancia del administrador de almacenes
        private readonly Admin admin = new Admin(); // Instancia del administrador

        /// <summary>
        /// Constructor de la clase <see cref="SellViewerHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public SellViewerHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SudoSellViewer" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y muestra las ventas del día si el usuario está autenticado como administrador.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            _manager.SellList();
            if (userfound != null && userfound.logged && userfound.admin)
            {
                if (_manager.sells.Count == 0)
                {
                    response = "No hay ventas registradas";
                }
                else
                {
                    // Enviar las ventas del día
                    TelegramBot.SendTextMessageAsync("Las ventas del día son:");
                    Thread.Sleep(50); // Pausa breve para asegurar la correcta entrega de mensajes
                    foreach (var sell in _manager.sells)
                    {
                        TelegramBot.SendTextMessageAsync($"{sell.Product} --> {sell.Quantity}\n", message.From.Id);
                    }
                    response = string.Empty;
                }
            }
            else
            {
                // Mensaje de error si el usuario no está logueado o no es un administrador
                response = "No puedo ejecutar este comando porque no estás loggeado o tu cuenta no está admitida como administrador. Por favor, escribe /LoginSession para loggearte o ve al menú correspondiente: Supplier: /MenuSupplier - User: /UserMenu";
               
            }
        }
    }
}
