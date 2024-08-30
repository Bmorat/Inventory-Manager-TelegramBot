using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de recibir el primer mensaje para la creación de un nuevo depósito.
    /// </summary>
    public class PreAddWarehouseHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="PreAddWarehouseHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public PreAddWarehouseHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoAddW" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido y solicita al usuario el nombre y la dirección del nuevo depósito.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            // Solicita al usuario que ingrese el nombre y la dirección del nuevo depósito
            response = "Escriba /AddWh e ingrese el nombre del depósito y la dirección del mismo. Ejemplo: /AddWh Central 8 de octubre y estero bellaco";
        }
    }
}
