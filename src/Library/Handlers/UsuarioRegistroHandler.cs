using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de registrar un usuario.
    /// </summary>
    public class UsuarioRegistroHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="UsuarioRegistroHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public UsuarioRegistroHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/RegUser" };
        }

        /// <summary>
        /// Maneja el mensaje de registro de un usuario.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            // Patrón para validar el formato del comando de registro de usuario
            string patron = @"^/RegUser\s+(\S+)\s+(\S+)\s+(\S+)$";
            Match match = Regex.Match(message.Text, patron);
            
            // Extraer los grupos del patrón
            string nombre = match.Groups[1].Value;
            string usuario = match.Groups[2].Value;
            string contraseña = match.Groups[3].Value;
            long Chat = message.From.Id;

            // Crear un nuevo objeto User y agregarlo a la lista de espera de usuarios
            User user = new User(nombre)
            {
                User = usuario,
                Password = contraseña,
                admin = false,
                logged = false,
                ChatId = Chat
            };

            admin.UserManager.UserWaitlist.Add(user);

            // Mensaje de respuesta para el usuario
            response = $"🤖 Registro Exitoso\nBienvenido {user.Name}\nAhora espera a que un administrador te ingrese en la plataforma\nTe notificaré cuando puedas usar la app";
        }
    }
}
