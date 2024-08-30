using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de recibir el primer mensaje para agregar items a un depósito.
    /// </summary>
    public class AddItemHandler : BaseHandler 
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase AddItemHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public AddItemHandler(BaseHandler next) : base(next)
        {
            Contract.Requires(next != null, "El siguiente handler no puede ser nulo.");
            Keywords = new[] { "/AddItemDeposito" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                Contract.Requires(message != null, "El mensaje no puede ser nulo.");
                Contract.Requires(!string.IsNullOrEmpty(message.Text), "El mensaje no puede estar vacío.");

                User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

                if (userfound == null || !userfound.logged)
                {
                    response = "Debes estar conectado para utilizar la aplicación. Por favor inicia sesión con /LoginSesion";
                    return;
                }

                string[] partes = message.Text.Split(new string[] { "/AddItemDeposito" }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 0 || string.IsNullOrWhiteSpace(partes[0]))
                {
                    throw new ArgumentException("El formato del mensaje es incorrecto. Debe ser: /AddItemDeposito nombre_del_deposito.");
                }
                _manager.JsonToList();
                string name = partes[0].Trim();
                name = StringExtensions.Capitalize(name);
                
                Warehouse warehousefound = _manager.Warehouses.FirstOrDefault(x => x.Name == name);

                if (warehousefound != null)
                {
                    _manager.AuxiliarWarehouse = warehousefound;
                    response = "Por favor escriba /AddinW seguido del nombre, la categoría y la cantidad. Ejemplo: /AddinW pollo comida 10";
                }
                else
                {
                    response = "Error, depósito no encontrado. Por favor, escriba /AddItemDeposito e ingrese el nombre del depósito. Ejemplo: /AddItemDeposito Central";
                }
            }
            catch (Exception ex)
            {
                
                response="Debes estar conectado para utilizar la aplicación por favor inicia sesión /LoginSesion";
            }
        }
    }
}
