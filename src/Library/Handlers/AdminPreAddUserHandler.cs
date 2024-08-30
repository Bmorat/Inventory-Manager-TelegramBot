using System;
using System.Collections.Generic;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el pre mensaje de los usuarios en espera de aprobación.
    /// </summary>
    public class AdminUserPreRegistroHandler : BaseHandler
    {
        Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase AdminUserPreRegistroHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public AdminUserPreRegistroHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoAddUser" };
        }

        /// <summary>
        /// Método que maneja el mensaje de mostrar los usuarios en espera para ser aceptados.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Admin admin = new Admin();
            List<User> WaitingUsers = admin.UserManager.GetWaitingUsers();
            List<Supplier> WaitingSupp = admin.UserManager.GetWaitingSuppliers();

            if (WaitingUsers.Count != 0)
            {
                TelegramBot.SendTextMessageAsync("Usuarios en espera:", message.From.Id);
                Thread.Sleep(50);
                foreach (User usuario in WaitingUsers)
                {
                    TelegramBot.SendTextMessageAsync($"Nombre: {usuario.Name} Usuario: {usuario.User}", message.From.Id);
                }
                Thread.Sleep(50);
                TelegramBot.SendTextMessageAsync("Por favor escribe /SudoUserRegister Nombre User\nEjemplo: /SudoUserRegister botname botuser", message.Chat.Id);
            }
            else if (WaitingSupp.Count != 0)
            {
                TelegramBot.SendTextMessageAsync("Proveedores en espera:", message.From.Id);
                Thread.Sleep(50);
                foreach (Supplier supp in WaitingSupp)
                {
                    TelegramBot.SendTextMessageAsync($"Nombre: {supp.Name} Usuario: {supp.User}", message.From.Id);
                }
                Thread.Sleep(50);
                TelegramBot.SendTextMessageAsync("Por favor escribe /SudoSuppRegister Nombre User\nEjemplo: /SudoSuppRegister botname botuser", message.Chat.Id);
            }
            else
            {
                TelegramBot.SendTextMessageAsync("No hay usuarios en espera. Presiona /SudoMenu para volver al menú principal", message.From.Id);
            }

            response = string.Empty;
        }
    }
}
