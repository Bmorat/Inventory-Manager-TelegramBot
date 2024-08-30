using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el pre mensaje de los usuarios en espera de aprobación.
    /// </summary>
    public class AdminAddUserHandler : BaseHandler
    {
        Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase AdminAddUserHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public AdminAddUserHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoUserRegister" };
        }

        /// <summary>
        /// Método que maneja el mensaje de mostrar los usuarios en espera para ser aceptados.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Admin admin = new Admin();
            string patron = @"^/SudoUserRegister\s+(\S+)\s+(\S+)$";
            Match match = Regex.Match(message.Text, patron);
            string name = match.Groups[1].Value;
            string user = match.Groups[2].Value;

            admin.UserManager.JsonToList();
            User user1 = admin.UserManager.UserWaitlist.Find(x => x.Name == name && x.User == user);

            if (user1 != null)
            {
                user1.admin = false;
                user1.logged = false;
                admin.AddUser(user1);
                admin.UserManager.UserWaitlist.Remove(user1);
                admin.UserManager.SaveChanges();

                response = "Usuario autorizado con éxito";

                user1.logged = true;
                response = "Has sido registrado por un administrador, por favor inicia sesión con tu usuario y contraseña. Utiliza el comando /LoginSesion";
            }
            else
            {
                response = "Usuario no encontrado en la lista de espera";
            }
        }
    }
}
