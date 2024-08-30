using System;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de registrar un proveedor.
    /// </summary>
    public class SupplierRegistroHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="SupplierRegistroHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public SupplierRegistroHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SuppReg" };
        }
        
        /// <summary>
        /// Maneja el mensaje de registro de un proveedor y lo agrega a la lista de espera de proveedores.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                // Expresión regular para validar el formato del mensaje
                string patron = @"^/SuppReg\s+(\S+)\s+(\S+)\s+(\S+)$";
                Match match = Regex.Match(message.Text, patron, RegexOptions.IgnoreCase);

                // Extraer los grupos de datos del mensaje
                string nombre = match.Groups[1].Value;
                string usuario = match.Groups[2].Value;
                string contraseña = match.Groups[3].Value;
                long id = message.From.Id;

                // Crear un nuevo proveedor con los datos extraídos
                Supplier supplier = new Supplier(nombre)
                {
                    User = usuario,
                    Password = contraseña,
                    admin = false,
                    logged = false,
                    ChatId = id
                };

                // Añadir el proveedor a la lista de espera
                admin.UserManager.SupplierWaitlist.Add(supplier);

                // Enviar un mensaje de confirmación al usuario
                response = $"🤖 Registro Exitoso\nBienvenido {supplier.Name}\nAhora espera a que un administrador te ingrese en la plataforma";

                response = String.Empty;
            }
            catch (Exception ex)
            {
                // Manejo de errores y asignación de mensaje de respuesta en caso de excepción
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
