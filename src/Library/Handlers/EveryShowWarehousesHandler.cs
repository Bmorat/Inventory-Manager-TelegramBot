using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el primer mensaje para ver los depósitos.
    /// </summary>
    public class EveryShowWarehousesHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance; // Instancia de la clase WarehouseManager
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase EveryShowWarehousesHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public EveryShowWarehousesHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SudoShowW" };
        }

        /// <summary>
        /// Método encargado de solicitar cuál depósito se desea ver y enviarla al siguiente handler encargado de mostrar los depósitos.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            if (userfound.logged == true)
            {
                _manager.JsonToList();
                var warehouses = _manager.Warehouses;

                if (warehouses == null || warehouses.Count == 0)
                {
                    response = "No hay depósitos disponibles.";
                    return;
                }

                response = "Sus depósitos son:\n";
                int n = 1;
                foreach (var warehouse in warehouses)
                {
                    response += $"{n}. {warehouse.Name} en {warehouse.Address}\n";
                    n++;
                }
                TelegramBot.SendTextMessageAsync("Si deseas volver al menú principal escribe /SudoWMenu para volver al menú principal", message.Chat.Id);
            }
            else
            {
                response = "Error: debes loguearte para utilizar esta función.";
            }
        }
    }
}
