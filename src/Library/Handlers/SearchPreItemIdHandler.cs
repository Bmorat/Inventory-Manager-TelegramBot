using System;
using System.Diagnostics.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Clase que se encarga de preparar la búsqueda de un ítem por ID.
    /// </summary>
    public class SearchPreItemIdHandler : BaseHandler
    {
        private readonly Admin admin = new Admin(); // Instancia del administrador

        /// <summary>
        /// Constructor de la clase <see cref="SearchPreItemIdHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public SearchPreItemIdHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SearchItemID" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y solicita el ID del artículo para buscar en el inventario.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                Contract.Requires(message != null, "El mensaje no puede ser nulo.");
                Contract.Requires(message.From != null, "El remitente del mensaje no puede ser nulo.");

                Supplier suppfound = admin.UserManager.RegisteredSuppliers.Find(x => x.ChatId == message.From.Id);
                User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

                if (userfound != null && userfound.logged)
                {
                    // Usuario registrado y logueado
                    response = "Ingrese /SearchID más el ID del artículo para ver el Stock:\nEjemplo: /SearchID 15";
                    return;
                }
                else if (suppfound != null && suppfound.logged)
                {
                    // Proveedor registrado y logueado
                    response = "Ingrese /SearchID más el ID del artículo para ver el Stock:\nEjemplo: /SearchID 15";
                    return;
                }
                else
                {
                    // Usuario no registrado o no logueado
                    response = "No puedo ejecutar este comando porque no estás logueado como usuario, admin o proveedor. Por favor, escribe /LoginSession para iniciar sesión.";
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
