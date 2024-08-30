using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// En este handler se recibe y se maneja el nombre del deposito recibido a partir de los jsons para mostrar al usuario la cantidad de items que tiene el deposito
    /// </summary>
    public class ItemQuantityHandler : BaseHandler
    {

        WarehouseManager _manager = Singleton<WarehouseManager>.Instance;
        Admin admin = new Admin();
        public ItemQuantityHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new[] { "/ItemQuantity" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            Contract.Requires(message != null, "El mensaje no puede ser nulo.");
            Contract.Requires(!string.IsNullOrEmpty(message.Text), "El mensaje no puede estar vacío.");
            try
            {   User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
                if (userfound !=null)//pedimos que solamente puedan ser los admins que implementen esta userstory
                {   
                    if(userfound.admin==true)
                    {
                        string patron = @"^/ItemQuantity\s+(\S+)$";//establecemos el patron para que quite el comando y obtengamos el nombre del deposito
                        Match match = Regex.Match(message.Text, patron);
                        if (!match.Success)
                        {
                            throw new ArgumentException("El formato del mensaje es incorrecto. Debe ser: /ItemQuantity + WarehouseName");
                        }
                        string name = match.Groups[1].Value;//nos quedamos con el nombre del deposito y lo capitalizamos
                        name = StringExtensions.Capitalize(name);
                        _manager.JsonToList();//traemos la informacion del json de los depositos a las listas
                        Warehouse foundwarehouse = _manager.Warehouses.Find(x => x.Name == name);
                        foundwarehouse.JsonToList();//traemos la informacion serializada en el json a la lista de secciones para poder obtener los items que ya estaban guardados
                        int ItemQuantity = 0;//establecemos una variable para poder contar los items y si esta en 0 se le devolvera al usuario que no hay items en el deposito
                        if (foundwarehouse != null)
                        {
                            foreach (var section in foundwarehouse.Sections)
                            {
                                if (section.warehouse==name)
                                {
                                        foreach(var item in section.items)
                                    {
                                        ItemQuantity+=item.Value.Quantity;//ingresamos al valor del diccionaro que es un objeto de item stock que tiene id nombre y cantidad y sumamos la cantidad
                                    }
                                }
                            }

                        }
                        else
                        {
                            response="Error deposito no encontrado";
                        }
                        if (ItemQuantity ==0)
                        {
                            response="No hay items que contar en este deposito";
                        }
                        else
                        {
                            response=$"En el deposito {name} tienes un total de {ItemQuantity} items";
                        }
                    }
                    else
                    {
                        response="Debes tener permisos de adminsitrador para usar esta función";
                    }
                }
                else
                {
                    response="Error Usuario no encontrado";
                }

            }  
            catch (Exception ex)
            {
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }


           
        }
    }
}
