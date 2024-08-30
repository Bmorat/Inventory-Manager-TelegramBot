using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Diagnostics.Contracts;

namespace Library
{
    /// <summary>
    /// Handler encargado de remover ítems de un depósito.
    /// </summary>
    public class RemoveItemHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance; // Instancia del administrador de depósitos
        private readonly Admin admin = new Admin(); // Instancia del administrador de usuarios

        /// <summary>
        /// Constructor de la clase <see cref="RemoveItemHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public RemoveItemHandler(BaseHandler next) : base(next)
        {
            Contract.Requires(next != null, "El siguiente handler no puede ser nulo.");
            Keywords = new[] { "/RemoveItem" };
        }

        /// <summary>
        /// Maneja el mensaje recibido para remover un ítem de un depósito.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Contract.Requires(message != null, "El mensaje no puede ser nulo.");
            Contract.Requires(message.From != null, "El remitente del mensaje no puede ser nulo.");

            try
            {
                User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

                if (userfound == null)
                {
                    throw new InvalidOperationException("Usuario no encontrado.");
                    response = "Usuario no encontrado.";
                }

                if (!userfound.logged)
                {
                    response = "No puedo ejecutar este comando ya que no estás loggeado como un usuario o administrador. Por favor, escribe \"/LoginSession\" para continuar.";
                    return;
                }

                // Lee el texto del mensaje recibido
                string name = TelegramBot.TelegramReadLine();
                if (string.IsNullOrEmpty(message.Text))
                {
                    throw new ArgumentException("El mensaje no contiene texto.");
                }

                // Extrae el nombre del depósito del mensaje
                string[] partes = message.Text.Split(new string[] { "/RemoveItem" }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 0)
                {
                    throw new ArgumentException("El mensaje no contiene la información necesaria.");
                }

                name = partes[0].Trim();
                name = StringExtensions.Capitalize(name);
                Warehouse warehouse = _manager.Warehouses.Find(x => x.Name == name);
        

                if (warehouse == null)
                {
                    response = "Error: depósito no encontrado.";
                    return;
                }

                _manager.AuxiliarWarehouse = warehouse;
                response = "Escriba /RemoveInW Sección Id Cantidad, Ejemplo: /RemoveInW Hogar 0 10";
            }
            catch (Exception ex)
            {
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
