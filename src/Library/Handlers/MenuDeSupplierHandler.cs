using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el menú para proveedores.
    /// </summary>
    public class MenuDeSupplierHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="MenuDeSupplierHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public MenuDeSupplierHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/MenuSupplier" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido y muestra el menú para proveedores.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            response = "---------------------------------------------------------------------------------------------\n" +
                       "\nBienvenido Proveedor, ¿qué deseas realizar en la Sección Almacenes?\n" +
                       "\n1. Mostrar Depósitos  📦             ➡️  /SudoShowW\n" +
                       "\n2. Buscar el stock de un artículo ➡️  /SearchItemID\n" +
                       "\n3. Para cerrar sesión 🚪               ➡️  /Exit\n" +
                       "\n---------------------------------------------------------------------------------------------";
        }
    }
}
