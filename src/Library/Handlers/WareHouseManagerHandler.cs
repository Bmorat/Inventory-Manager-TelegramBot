using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de mostrar las opciones de la sección de almacenes de Administrador.
    /// </summary>
    public class WareHouseManagerHandler : BaseHandler
    {
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase <see cref="WareHouseManagerHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public WareHouseManagerHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/SudoWMenu" };
        }

        /// <summary>
        /// Maneja el mensaje recibido y proporciona las opciones disponibles en la sección de almacenes.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario, con las opciones de gestión de almacenes.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            response = "---------------------------------------------------------------------------------------------\n" +
                       "\nBienvenido a la Sección de depósitos. ¿Qué deseas hacer?\n" +
                       "\n1_Mostrar Depósitos  📦                   ➡️  /SudoShowW\n" +
                       "\n2_Agregar Depósitos    📦                  ➡️  /SudoAddwh\n" +
                       "\n3_Agregar Artículo        📥                  ➡️  /SudoAddItem\n" +
                       "\n4_Remover Artículo (venta)     📤     ➡️  /SudoRemoveItem\n" +
                       "\n5_Ver inventarios      🗂                      ➡️  /SudoShowInventory\n" +
                       "\n6_Ver ventas            💵                        ➡️  /SudoSellViewer\n" +
                       "\n7_Volver al menú principal   📖       ➡️  /SudoMenu\n" +
                       "\n---------------------------------------------------------------------------------------------";
        }
    }
}
