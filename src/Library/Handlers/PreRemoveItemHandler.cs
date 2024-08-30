using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de preparar el proceso para remover ítems de un depósito.
    /// </summary>
    public class PreRemoveItemHandler : BaseHandler
    {
        private readonly WarehouseManager _manager;
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="PreRemoveItemHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public PreRemoveItemHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SudoRemoveItem" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y solicita al usuario que ingrese el nombre del depósito para proceder con la eliminación de ítems.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            // Verifica si el usuario está loggeado
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            if (userfound != null && userfound.logged)
            {
                // Solicita al usuario que ingrese el nombre del depósito
                response = "Escriba /RemoveItem e ingrese el nombre del depósito. Ejemplo: /RemoveItem Central";
            }
            else
            {
                // Informa al usuario que debe estar loggeado
                response = "No puedo ejecutar este comando ya que no estás loggeado como un usuario o administrador. Por favor escribe \"/LoginSession\" para continuar.";
            }
        }
    }
}
