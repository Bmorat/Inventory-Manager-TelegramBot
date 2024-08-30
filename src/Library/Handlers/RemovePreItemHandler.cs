using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Library
{
    public class RemovePreItemHandler : BaseHandler //Handler encargado de remover items de un deposito
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;

        public RemovePreItemHandler(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/RemoveItem" };
        }

        protected override void InternalHandle(Message message, out string response)
        {
            response = "Escribe /RemoveFromW NombreItem Q Cantidad seguido del nombre del item a remover y la cantidad a remover\nEjemplo: /RemoveFromW NombreItem Q 5";
        }
    }
}