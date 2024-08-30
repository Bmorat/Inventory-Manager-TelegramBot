using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el mensaje de inicio de sesión para usuarios o proveedores.
    /// </summary>
    public class SuppOrUserHandler : BaseHandler
    {
        private readonly ITelegramBotClient BotClient;
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="SuppOrUserHandler"/>.
        /// </summary>
        /// <param name="BotClient">Cliente de Telegram bot para enviar mensajes.</param>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public SuppOrUserHandler(ITelegramBotClient BotClient, BaseHandler next) : base(next)
        {
            Keywords = new[] { "/Registrarse" };
            this.BotClient = BotClient;
        }
        
        /// <summary>
        /// Maneja el mensaje recibido y muestra el mensaje de inicio de sesión para usuarios o proveedores.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            // Mensaje que se enviará al usuario con las opciones de registro
            response = "-----------------------------------\n" +
                        "🤖 Bienvenido, voy a ayudarte con tu registro\n" +
                        "Si deseas registrarte como usuario 🧑🏻, elige 1; si deseas registrarte como proveedor 🚚, elige 2\n" +
                        "Por favor, elige una opción\n" +
                        "--------------------------------------\n" +
                        "/Users para registrarte como usuario\n" +
                        "/Proveedores para registrarte como proveedor\n";

            // Mensaje para el teclado con botones
            string mensaje = response;
            response = string.Empty;

            // Creación de un teclado con botones
            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("/Users"),
                new KeyboardButton("/Proveedores")
            })
            {
                ResizeKeyboard = true, // Ajusta el tamaño del teclado para que se ajuste al espacio disponible
                OneTimeKeyboard = true // Oculta el teclado después de que el usuario lo use
            };

            // Enviar el mensaje con el teclado
            this.BotClient.SendTextMessageAsync(message.Chat.Id, mensaje, replyMarkup: keyboard);
        }
    }
}
