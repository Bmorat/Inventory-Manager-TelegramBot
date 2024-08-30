using System;
using System.Diagnostics.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado del mensaje inicial.
    /// </summary>
    public class HelloHandler : BaseHandler
    {
        private readonly ITelegramBotClient BotClient;
        

        /// <summary>
        /// Constructor de la clase HelloHandler.
        /// </summary>
        /// <param name="BotClient">El cliente del bot para enviar mensajes.</param>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public HelloHandler(ITelegramBotClient BotClient, BaseHandler next) : base(next)
        {
            Contract.Requires(BotClient != null, "El cliente del bot no puede ser nulo.");
            Contract.Requires(next != null, "El siguiente handler no puede ser nulo.");

            this.Keywords = new[] { "/iniciar", "/start", "/Start", "/Iniciar", "Hola", "hola" };
            this.BotClient = BotClient;
        }

        /// <summary>
        /// Método que maneja el mensaje de bienvenida y envía las opciones de iniciar sesión o registrarse.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                Contract.Requires(message != null, "El mensaje no puede ser nulo.");

                // Mensaje de bienvenida con opciones para iniciar sesión o registrarse.
                response = "---------------------------------------------------------------------------------------------\n" +
                           "  👋🏻 Hola, soy el Bot 🤖 de EZ Inventory del Grupo N°5\n" +
                           "\nVoy a ayudarte a gestionar tu Inventario 📊 📈 🛒\n" +
                           "\nBienvenido a EZ Inventory, para iniciar sesión elige 1, para\n" +
                           "\nregistrarte elige 2\n" +
                           "\nPor favor elige una opción: \n" +
                           "\n---------------------------------------------------------------------------------------------\n" +
                           "\n➡️     /LoginSesion para iniciar sesión\n" +
                           "\n➡️     /Registrarse para registrarte";

                // Se vacía el contenido de la variable response para evitar que se mande el mensaje doble.
                string mensaje = response;
                response = string.Empty;

                // Creación de un teclado con botones.
                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("/LoginSesion"),
                    new KeyboardButton("/Registrarse")
                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                // Envío del mensaje con el teclado.
                this.BotClient.SendTextMessageAsync(message.Chat.Id, mensaje, replyMarkup: keyboard).Wait();
            }
            catch (Exception ex)
            {
                // En caso de error, se envía un mensaje de error y se imprime el detalle en la consola.
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}. Por favor escribe 'hola' para intentar de nuevo.";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
