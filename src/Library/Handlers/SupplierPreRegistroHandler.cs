using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el primer mensaje para el registro de un proveedor.
    /// </summary>
    public class SupplierPreRegistroHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="SupplierPreRegistroHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public SupplierPreRegistroHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/Proveedores" };
        }
        
        /// <summary>
        /// Maneja el mensaje recibido y solicita información para el registro de un proveedor.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                // Validación de que el mensaje y su remitente no sean nulos
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message), "El mensaje no puede ser nulo.");
                }

                if (message.From == null)
                {
                    throw new ArgumentNullException(nameof(message.From), "El remitente del mensaje no puede ser nulo.");
                }

                Supplier suppFound = admin.UserManager.RegisteredSuppliers.Find(x => x.ChatId == message.From.Id);
                User userFound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

                // Verificar si el usuario o proveedor está registrado y logueado
                if (userFound != null && userFound.logged || suppFound != null && suppFound.logged)
                {
                    response = "Hola Proveedor! 🚚, a continuación necesito que ingreses /SuppReg Nombre Usario Contraseña\nEjemplo: /SuppReg botname botuser botpass";
                }
                else
                {
                    // Enviar un mensaje si el usuario no está logueado
                    TelegramBot.SendTextMessageAsync("No puedo ejecutar este comando porque no estás loggeado como usuario, proveedor o administrador. Por favor, escribe /Exit para cerrar sesión y un Hola para registrarse.", message.From.Id);
                    response = String.Empty;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores y asignación de mensaje de respuesta en caso de excepción
                response = "Se produjo un error al manejar el mensaje";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
