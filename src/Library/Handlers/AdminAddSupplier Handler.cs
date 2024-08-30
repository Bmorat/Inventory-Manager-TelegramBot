using System;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el pre mensaje de los usuarios en espera de aprobación.
    /// </summary>
    public class AdminAddSupplierHandler : BaseHandler
    {
        /// <summary>
        /// Constructor de la clase AdminAddSupplierHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public AdminAddSupplierHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoSuppRegister" };
        }

        /// <summary>
        /// Método que maneja el mensaje de mostrar los usuarios en espera para ser aceptados.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Admin admin = new Admin();
            string patron = @"^/SudoSuppRegister\s+(\S+)\s+(\S+)$";
            Match match = Regex.Match(message.Text, patron);
            string name = match.Groups[1].Value;
            string user = match.Groups[2].Value;

            admin.UserManager.JsonToList();
            Supplier user1 = admin.UserManager.SupplierWaitlist.Find(x => x.Name == name && x.User == user);

            if (user1 != null)
            {
                user1.admin = false;
                user1.logged = false;
                admin.AddSupplier(user1);
                admin.UserManager.SupplierWaitlist.Remove(user1);
                admin.UserManager.SaveChanges();

                TelegramBot.SendTextMessageAsync("Usuario autorizado con éxito", message.From.Id);

                user1.logged = true;
                TelegramBot.SendTextMessageAsync("Has sido registrado por un administrador por favor inicia sesión con tu usuario y contraseña, utiliza el comando /LoginSesion", user1.ChatId);
            }
            else
            {
                TelegramBot.SendTextMessageAsync("Usuario no encontrado en la lista de espera", message.From.Id);
            }

            response = string.Empty;
        }
    }
}
