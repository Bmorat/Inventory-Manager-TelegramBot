using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el mensaje de inicio de sesión.
    /// </summary>
    public class IniciarSesionHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="IniciarSesionHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public IniciarSesionHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/LoginSesion" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido y muestra las instrucciones para iniciar sesión.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            response = "---------------------------------------------------------------------------------------------\n" +
                       "\n🤖 Por favor ingrese su usuario y contraseña escriba '/Login'\n" +
                       "\nseguido de su nombre de usuario 🙋🏻‍♂️ y seguido su contraseña\n" +
                       "\nEjemplo: /Login botuser botpassword 😎\n" +
                       "\n---------------------------------------------------------------------------------------------\n" +
                       "\n➡️     /Login + Usuario + Contraseña";
        }
    }
}
