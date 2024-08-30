using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    public class ShowWarehousesHandler : BaseHandler //Handler encargado de mostrar los depositos
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance; //Instancia de la clase WarehouseManager

        public ShowWarehousesHandler(BaseHandler next) : base(next) //Constructor de la clase
        {
            Keywords = new[] { "/SudoShowWName" };
        }

        protected override void InternalHandle(Message message, out string response) //Metodo encargado de solicitar cual deposito se desea ver
        {
            response = "Ingrese el nombre del depósito que desea ver:";
            //ACA VA MOSTRAR EL DEPOSITO ESPECIFICO Y QUE TIENE ADENTRO
        }
    }
}
