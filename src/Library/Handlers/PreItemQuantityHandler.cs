using System;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Voy a agregar dos handlers nuevos para poder obtener la cantidad de items en un deposito deseado, en el primero se ingresara a la logica
    /// donde se pedira al usuario que ingrese el nombre del deposito en el cual desea obtener la cantidad de items, donde pasara al siguiente
    /// handler que devolvera finalmente el resultado
    /// </summary>
    public class PreItemQuantityHandler : BaseHandler
    {

        public PreItemQuantityHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/WItemQuantity" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            response = "🤖: Ingrese /ItemQuantity + el nombre del deposito en el cual desea obtener la cantidad de items\nEjemplo: /ItemQuantity Central";
        }
    }
}
