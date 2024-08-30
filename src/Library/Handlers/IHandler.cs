using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Interfaz que define los métodos que deben tener los handlers.
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Propiedad que define el siguiente handler en la cadena de responsabilidad.
        /// </summary>
        IHandler Next { get; set; }

        /// <summary>
        /// Método para cancelar la operación actual.
        /// </summary>
        /// <param name="message">El mensaje asociado con la operación que se desea cancelar.</param>
        void Cancel(Message message);

        /// <summary>
        /// Método para manejar el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        /// <returns>El siguiente handler que puede manejar el mensaje, o <c>null</c> si no se puede manejar.</returns>
        IHandler Handle(Message message, out string response);
    }
}
