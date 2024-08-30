using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Clase para gestionar las interacciones con un bot de Telegram. (Bot CORE)(no implementado aún)
    /// </summary>
    public class TelegramBot 
    {
        private static TelegramBotClient bot;
        
        public static bool IsLogged=false;
        /// <summary>
        /// Identificador del chat actual.
        /// </summary>
        public static long ChatId { get; private set; }

        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// </summary>
        private static IHandler ? firstHandler;
       

        public static void Main()
        {
            bot = new TelegramBotClient("7353367298:AAFOA7_Ee6OXlRwSlXy-Xb5tTvhFNnUPayw"); // Token del Bot de telegram obtenido mediante BotFather

            LocationApiClient client = new LocationApiClient();
            IAddressFinder finder = new AddressFinder(client);

            firstHandler = 
            new HelloHandler(bot,
            new IniciarSesionHandler(
            new LoginUsuarioHandler(bot,
            new SuppOrUserHandler(bot,
            new UsuariosPreRegistroHandler(
            new ExitHandler(
            new SupplierPreRegistroHandler(
            new SupplierRegistroHandler(
            new AdminAddSupplierHandler(
            new UsuarioRegistroHandler(
            new MenuDeSupplierHandler(
            new WareHouseManagerHandler(
            new EveryShowWarehousesHandler(
            new SearchPreItemIdHandler(
            new SearchItemIdHandler( client,
            new PreShowInventoryHandler(
            new AddItemHandler2(
            new AfterRemoveItemHandler(
            new AdminAddUserHandler(
            new PreShowInventoryHandler(
            new PreAddItemHandler(
            new AddItemHandler(
            new SellViewerHandler(
            new MenuDeUserHandler(
            new PreRemoveItemHandler(
            new RemoveItemHandler(
            new ShowInventoryHandler(
            new MenudeAdminHandler(
            new AdminUserPreRegistroHandler(
            new PreAddWarehouseHandler(
            new AddWarehouseHandler(finder, bot,
            new PreItemQuantityHandler(
            new ItemQuantityHandler(null) ))
            ))))))))))))))))))))))))))))));
        
    

            

            var cts = new CancellationTokenSource();

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                },
                cts.Token
            );

            Console.WriteLine($"Bot is up!");

            Console.ReadLine();

            cts.Cancel();
        }

        /// <summary>
        /// Maneja las actualizaciones recibidas por el bot.
        /// </summary>
        /// <param name="botClient">Cliente del bot de Telegram.</param>
        /// <param name="update">Actualización recibida.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tarea asíncrona.</returns>
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            try
            {
                if (update.Type == UpdateType.Message)
                {
                    var message = update.Message;
                    if (message != null)
                    {
                        switch (message.Type)
                        {
                            case MessageType.Text:
                                await HandleMessageReceived(botClient, message);
                                break;
                            case MessageType.Location:
                                await HandleLocationAsync(botClient, message);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await HandleErrorAsync(botClient, e, cancellationToken);
            }
        }

        /// <summary>
        /// Maneja los mensajes recibidos por el bot.
        /// </summary>
        /// <param name="botClient">Cliente del bot de Telegram.</param>
        /// <param name="message">Mensaje recibido.</param>
        /// <returns>Tarea asíncrona.</returns>
        private static async Task HandleMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Received a message from {message.From.FirstName} saying: {message.Text}");

            string response = String.Empty;
            // Aca se procesará el mensaje y se generará una respuesta.
            firstHandler.Handle(message, out response); 

            await botClient.SendTextMessageAsync(message.Chat.Id, response);
            ChatId = message.From.Id;
        }

        /// <summary>
        /// Maneja los errores que ocurren durante la operación del bot.
        /// </summary>
        /// <param name="botClient">Cliente del bot de Telegram.</param>
        /// <param name="exception">Excepción que ocurrió.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Tarea completada.</returns>
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Envía un mensaje de texto al chat actual.
        /// </summary>
        /// <param name="message">Mensaje a enviar.</param>
        /// <returns>Tarea asíncrona.</returns>
        public static async Task SendTextMessageAsync(string message)
        {
            // Envía el mensaje al bot de Telegram.
            await bot.SendTextMessageAsync(ChatId, message);
        }

        /// <summary>
        /// Envía un mensaje de texto a un chat específico.
        /// </summary>
        /// <param name="message">Mensaje a enviar.</param>
        /// <param name="chatid">ID del chat al que se enviará el mensaje.</param>
        /// <returns>Tarea asíncrona.</returns>
        public static async Task SendTextMessageAsync(string message, long chatid)
        {
            // Envía el mensaje al bot de Telegram.
            await bot.SendTextMessageAsync(chatid, message);
        }

        public static string TelegramReadLine()
        {
        // Obtener el último mensaje del chat
        var updates = bot.GetUpdatesAsync().Result;
        var lastMessage = updates.LastOrDefault()?.Message;

        // Si hay un mensaje, devolver su texto
        if (lastMessage != null)
        {
            return lastMessage.Text;
        }

        // Si no hay mensaje, devolver una cadena vacía
        return "";
        }
        private static async Task HandleLocationAsync(ITelegramBotClient botClient, Message message)
        {
            
            if (message.Location != null && TelegramBot.IsLogged)
            {
                Admin admin = new Admin();
                // Extract latitude and longitude
                double latitude = message.Location.Latitude;
                double longitude = message.Location.Longitude;
                Console.WriteLine($"Received location: Latitude = {latitude}, Longitude = {longitude}");
                string responseText = $"Ubicación guardada con éxito 🌎";
                await botClient.SendTextMessageAsync(message.Chat.Id, responseText, replyMarkup: new ReplyKeyboardRemove());
                User FoundUser = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
                Supplier FoundSupp=admin.UserManager.RegisteredSuppliers.Find(x => x.ChatId == message.From.Id);
                if (FoundUser !=null)
                {
                    FoundUser.Longitude=longitude;
                    FoundUser.Latitude=latitude;
                    int index = admin.UserManager.RegisteredUsers.FindIndex(x => x.Name == FoundUser.Name);
                    if (index != -1)
                    {
                        admin.UserManager.RegisteredUsers[index]=FoundUser;
                    
                    }
                    admin.UserManager.SaveChanges();

                }
                if (FoundSupp !=null)
                {
                    FoundSupp.Longitude=longitude;
                    FoundSupp.Latitude=latitude;
                    int index = admin.UserManager.RegisteredSuppliers.FindIndex(x => x.Name == FoundSupp.Name);
                    if (index != -1)
                    {
                        admin.UserManager.RegisteredSuppliers[index]=FoundSupp;
                    
                    }
                    admin.UserManager.SaveChanges();

                }
                IsLogged=false;

               
            }
            else
            {
                TelegramBot.SendTextMessageAsync("Debes iniciar sesión para guardar tu ubicación actual",message.From.Id);
            }

            
        }





    }
}
