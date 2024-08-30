using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Clase abstracta que implementa la interfaz IHandler. Esta clase sirve como base para otros handlers.
    /// </summary>
    public abstract class BaseHandler : IHandler
    {
        /// <summary>
        /// Propiedad que indica el siguiente handler en la cadena de responsabilidad.
        /// </summary>
        public IHandler Next { get; set; }

        /// <summary>
        /// Palabras clave que este handler puede procesar.
        /// </summary>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Constructor de la clase BaseHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public BaseHandler(IHandler next)
        {
            this.Next = next;
        }

        /// <summary>
        /// Constructor de la clase BaseHandler con palabras clave.
        /// </summary>
        /// <param name="keywords">Palabras clave que este handler puede procesar.</param>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public BaseHandler(string[] keywords, BaseHandler next)
        {
            this.Keywords = keywords;
            this.Next = next;
        }

        /// <summary>
        /// Método abstracto que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected abstract void InternalHandle(Message message, out string response);

        /// <summary>
        /// Método que cancela el mensaje.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        protected virtual void InternalCancel(Message message)
        {
            // Intencionalmente en blanco
        }

        /// <summary>
        /// Método que verifica si el mensaje puede ser manejado.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <returns>Verdadero si el mensaje puede ser manejado, falso en caso contrario.</returns>
        /// <exception cref="InvalidOperationException">Si no hay palabras clave que puedan ser procesadas.</exception>
        protected virtual bool CanHandle(Message message)
        {
            // Cuando no hay palabras clave este método debe ser sobreescrito por las clases sucesoras y por lo tanto
            // este método no debería haberse invocado.
            if (this.Keywords == null || this.Keywords.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }
            return this.Keywords.Any(s => message.Text.Contains(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        /// <returns>El handler que ha manejado el mensaje.</returns>
        public IHandler Handle(Message message, out string response)
        {
            if (this.CanHandle(message))
            {
                this.InternalHandle(message, out response);
                return this;
            }
            else if (this.Next != null)
            {
                return this.Next.Handle(message, out response);
            }
            else
            {
                Console.WriteLine("Mensaje no manejado");
                response = "No puedo reconocer ese Comando por favor intenta de nuevo";
                return null;
            }
        }

        /// <summary>
        /// Método que cancela el manejo del mensaje.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        public virtual void Cancel(Message message)
        {
            this.InternalCancel(message);
            if (this.Next != null)
            {
                this.Next.Cancel(message);
            }
        }
    }
}
