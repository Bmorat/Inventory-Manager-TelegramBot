using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el menú para usuarios.
    /// </summary>
    public class MenuDeUserHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="MenuDeUserHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public MenuDeUserHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/UserMenu" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido y muestra el menú para usuarios.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            if (userfound != null && userfound.logged)
            {
                response = "---------------------------------------------------------------------------------------------\n" +
                           "\nBienvenido Usuario, ¿qué deseas realizar en la Sección Almacenes?\n" +
                           "\n1. Mostrar Depósitos  📦          ➡️  /SudoShowW\n" +
                           "\n2. Agregar Artículo        📥         ➡️  /SudoAddItem\n" +
                           "\n3. Buscar Producto por ID 🔍  ➡️  /SearchItemID\n" +
                           "\n4. Para cerrar sesión 🚪          ➡️  /Exit\n" +
                           "\n---------------------------------------------------------------------------------------------";
            }
            else
            {
                response = "No puedo ejecutar este comando ya que no estás logueado en tu cuenta. Por favor, escribe \"/LoginSession\" para continuar.";
            }
        }
    }
}
