using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Library
{
    /// <summary>
    /// Handler encargado de manejar el inicio de sesión de los usuarios.
    /// </summary>
    public class LoginUsuarioHandler : BaseHandler
    {
        private readonly ITelegramBotClient BotClient;

        /// <summary>
        /// Constructor de la clase <see cref="LoginUsuarioHandler"/>.
        /// </summary>
        /// <param name="BotClient">El cliente del bot de Telegram.</param>
        /// <param name="next">El siguiente handler en la cadena de responsabilidad.</param>
        public LoginUsuarioHandler(ITelegramBotClient BotClient, BaseHandler next) : base(next)
        {
            Contract.Requires(BotClient != null, "El cliente del bot no puede ser nulo.");
            Contract.Requires(next != null, "El siguiente handler no puede ser nulo.");

            this.Keywords = new[] { "/Login" };
            this.BotClient = BotClient;
        }

        /// <summary>
        /// Método que maneja el mensaje recibido para iniciar sesión.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                Contract.Requires(message != null, "El mensaje no puede ser nulo.");
                Contract.Requires(!string.IsNullOrEmpty(message.Text), "El mensaje no puede estar vacío.");

                Admin admin = new Admin();
                string patron = @"^/Login\s+(\S+)\s+(\S+)$";
                Match match = Regex.Match(message.Text, patron, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    throw new ArgumentException("El formato del mensaje es incorrecto. Debe ser: /Login usuario contraseña.");
                }

                string usuario = match.Groups[1].Value;
                string contraseña = match.Groups[2].Value;

                admin.UserManager.JsonToList();
                User FoundUser = admin.UserManager.RegisteredUsers.Find(x => x.User == usuario && x.Password == contraseña);
                Supplier SupplierFound = admin.UserManager.RegisteredSuppliers.Find(x => x.User == usuario && x.Password == contraseña);
                
                // Crear un teclado para solicitar la ubicación del usuario                
                var requestLocationKeyboard = new ReplyKeyboardMarkup(new KeyboardButton("Compartir Ubicación")
                {
                    RequestLocation = true
                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
                BotClient.SendTextMessageAsync(message.Chat.Id, "Por favor, comparte tu ubicación actual🗺️", replyMarkup: requestLocationKeyboard).Wait();

                if (FoundUser != null)
                {
                    TelegramBot.IsLogged = true;
            
                    // Notificar al usuario que ha iniciado sesión correctamente
                    TelegramBot.SendTextMessageAsync("---------------------------------------------------------------------------------------------\n" +
                        "\n🤖 Bienvenido " + $"{FoundUser.Name}\n" + "\n---------------------------------------------------------------------------------------------").Wait();
                    FoundUser.logged = true;
                    
                    // Actualizar la lista de usuarios registrados
                    int index = admin.UserManager.RegisteredUsers.FindIndex(x => x.Name == FoundUser.Name);
                    if (index != -1)
                    {
                        admin.UserManager.RegisteredUsers[index] = FoundUser;
                    }
                    admin.UserManager.SaveChanges();
                    
                    // Enviar mensaje dependiendo del tipo de usuario
                    if (FoundUser.admin == true)
                    {
                        TelegramBot.SendTextMessageAsync("---------------------------------------------------------------------------------------------\n" +
                            "\n🤖 Tienes permisos de administrador para continuar escribe\n" + "\n/SudoMenu\n" +
                            "\n---------------------------------------------------------------------------------------------\n" + "\n➡️     /SudoMenu").Wait();
                    }
                    else
                    {
                        TelegramBot.SendTextMessageAsync("---------------------------------------------------------------------------------------------\n" +
                            "\n🤖 Tienes permisos de usuario escribe /UserMenu para continuar" +
                            "\n---------------------------------------------------------------------------------------------\n" +
                            "\n➡️     /UserMenu").Wait();
                    }
                }
                else if (SupplierFound != null)
                {   
                    // Notificar al proveedor que ha iniciado sesión correctamente                    
                    TelegramBot.SendTextMessageAsync("---------------------------------------------------------------------------------------------\n" +
                        "\n🤖 Bienvenido " + $"{SupplierFound.Name}\n" + "\n---------------------------------------------------------------------------------------------", message.From.Id).Wait();
                    TelegramBot.SendTextMessageAsync("---------------------------------------------------------------------------------------------\n" +
                        "\n🤖 Tienes permisos de proveedor escribe /MenuSupplier para continuar" +
                        "\n---------------------------------------------------------------------------------------------\n" +
                        "\n➡️     /MenuSupplier").Wait();
                    SupplierFound.logged = true;
                    
                    // Actualizar la lista de proveedores registrados
                    int index = admin.UserManager.RegisteredSuppliers.FindIndex(x => x.Name == SupplierFound.Name);
                    if (index != -1)
                    {
                        admin.UserManager.RegisteredSuppliers[index] = SupplierFound;
                    }
                    admin.UserManager.SaveChanges();
                }
                else
                {   
                    // Notificar al usuario sobre errores en el inicio de sesión
                    TelegramBot.SendTextMessageAsync("❌❌Error\nNombre de Usuario o Contraseña incorrectos❌❌").Wait();
                    TelegramBot.SendTextMessageAsync("🤖 Por favor ingrese su usuario y contraseña escriba 'Usuario' seguido de su nombre de usuario\ny 'Pass' seguido de su contraseña\nUsuario Cono Pass Contraseña").Wait();
                }

                response = string.Empty;
            }
            catch (Exception ex)
            {
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
