using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar el menú de administrador.
    /// </summary>
    public class MenudeAdminHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="MenudeAdminHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public MenudeAdminHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/SudoMenu" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido y muestra el menú de administrador si el usuario está logueado y tiene permisos de administrador.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);

            if (userfound != null && userfound.logged && userfound.admin)
            {
                response = "---------------------------------------------------------------------------------------------\n" +
                           "\nBienvenido 👨🏻‍💻 Administrador, ¿qué deseas hacer?\n" +
                           "\n1. Mostrar Depósitos  📦                   ➡️  /SudoShowW\n" +
                           "\n2. Agregar Artículo    📥                      ➡️  /SudoAddItem\n" +
                           "\n3. Buscar Producto por ID 🔍           ➡️  /SearchItemID\n" +
                           "\n4. Administrar Depósitos       📊       ➡️  /SudoWMenu\n" +
                           "\n5. Agregar un Usuario            🤝       ➡️  /SudoAddUser\n" +
                           "\n6. Para cerrar sesión              🚪       ➡️  /Exit\n" +
                           "\n---------------------------------------------------------------------------------------------";
            }
            else
            {
                response = "No es posible ejecutar este comando ya que no estás loggeado en tu cuenta o tu cuenta no está registrada como administrador.";
            }
        }
    }
}
