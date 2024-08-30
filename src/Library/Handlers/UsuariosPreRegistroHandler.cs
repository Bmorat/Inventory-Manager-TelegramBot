using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el primer mensaje para el registro de un usuario.
    /// </summary>
    public class UsuariosPreRegistroHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="UsuariosPreRegistroHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public UsuariosPreRegistroHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/Users" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y solicita la información necesaria para el registro de un usuario.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            // Mensaje que solicita la información para el registro de un usuario
            response = "🤖 Hola Usuario, a continuación necesito que ingreses /RegUser Nombre Usuario Contraseña\nEjemplo /RegUser botname botuser botpass\n";
        }
    }
}
