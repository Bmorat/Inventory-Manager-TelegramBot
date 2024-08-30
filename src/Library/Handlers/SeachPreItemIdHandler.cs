using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    public class SeachPreItemIdHandler : BaseHandler //Clase que se encarga de buscar un item por ID
    {
        
        public SeachPreItemIdHandler(BaseHandler next) : base(next) //Constructor de la clase
        {
            this.Keywords = new[] { "/SearchItemID" };
            
        }
        // Método para manejar el mensaje recibido
        protected override void InternalHandle(Message message, out string response) //Metodo que maneja el mensaje recibido
        {
            response = "Ingrese /SearchID mas el ID del articulo para ver el Stock:\nEjemplo: /SearchID 15";
        }
    }
}