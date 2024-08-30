using System;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el mensaje de cierre de sesión.
    /// </summary>
    public class ExitHandler : BaseHandler
    {
        Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase ExitHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public ExitHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/Exit" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            admin.UserManager.JsonToList();
            long id = message.From.Id;
            User FoundUser = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == id);
            Supplier FoundSupp = admin.UserManager.RegisteredSuppliers.Find(x => x.ChatId == id);

            if (FoundUser != null)
            {
                FoundUser.logged = false;
                int index = admin.UserManager.RegisteredUsers.FindIndex(x => x.ChatId == FoundUser.ChatId);
                if (index != -1)
                {
                    admin.UserManager.RegisteredUsers[index] = FoundUser;
                    admin.UserManager.SaveChanges();
                }
            }
            else if (FoundSupp != null)
            {
                FoundSupp.logged = false;
                int index = admin.UserManager.RegisteredSuppliers.FindIndex(x => x.ChatId == FoundSupp.ChatId);
                if (index != -1)
                {
                    admin.UserManager.RegisteredSuppliers[index] = FoundSupp;
                    admin.UserManager.SaveChanges();
                }
            }

            response = "🤖: ¡Te has desconectado! ¡Hasta pronto! 👋";
        }
    }
}
