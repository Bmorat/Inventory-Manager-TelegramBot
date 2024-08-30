using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de recibir el primer mensaje para la creación de un nuevo ítem.
    /// </summary>
    public class PreAddItemHandler : BaseHandler
    {
        private readonly WarehouseManager _manager;
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="PreAddItemHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public PreAddItemHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoAddItem" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido y solicita al usuario el nombre del depósito para agregar un nuevo ítem.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            if (userfound != null && userfound.logged)
            {
                // Solicita al usuario que ingrese el nombre del depósito
                response = "Escriba /AddItemDeposito e ingrese el nombre del depósito, Ejemplo: /AddItemDeposito Central";
            }
            else
            {
                // Informa al usuario que debe iniciar sesión para utilizar la aplicación
                response = "Debes estar conectado para utilizar la aplicación. Por favor, inicia sesión con /LoginSesion";
            }
        }
    }
}
