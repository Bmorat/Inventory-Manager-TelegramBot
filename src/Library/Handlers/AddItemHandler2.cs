using System;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de agregar un item a un depósito.
    /// </summary>
    public class AddItemHandler2 : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;

        Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase AddItemHandler2.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public AddItemHandler2(BaseHandler next) : base(next)
        {
            Keywords = new[] { "/AddinW" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            User userfound = admin.UserManager.RegisteredUsers.Find(x => x.ChatId == message.From.Id);
            if (userfound.logged == true)
            {
                string patron = @"^/AddinW\s+(\S+)\s+(\S+)\s+(\d+)$";
                Match match = Regex.Match(message.Text, patron, RegexOptions.IgnoreCase);
                string name = match.Groups[1].Value;
                name = StringExtensions.Capitalize(name);
                string cat = match.Groups[2].Value;
                cat = StringExtensions.Capitalize(cat);
                int quant = Convert.ToInt32(match.Groups[3].Value);
                Warehouse w = _manager.AuxiliarWarehouse;
                Item item = new Item(name);
                w.JsonToList();
                item.Category = cat;
                Section section = w.Sections.FirstOrDefault(s => s.Name == cat && s.warehouse == w.Name );
                

                if (section == null)
                {
                    section = new Section(cat);
                    w.Sections.Add(section);
                    section.warehouse=w.Name;
                    
                }

                StockItem existingItem = section.items.Values.FirstOrDefault(si => si.Item.Name == name);
                if (existingItem != null)
                {
                    existingItem.Quantity += quant;
                }
                else
                {
                    int newId = section.items.Keys.DefaultIfEmpty(-1).Max() + 1;
                    section.items[newId] = new StockItem(newId, item, quant);
                }

                w.SaveChanges();
                TelegramBot.SendTextMessageAsync($"Item agregado exitosamente. {item.Name} en el depósito {w.Name} en la sección {section.Name}", message.Chat.Id);

                // Actualiza el depósito en la lista original
                int index = _manager.Warehouses.FindIndex(x => x.Name == w.Name);
                if (index != -1)
                {
                    _manager.Warehouses[index] = w;
                    _manager.SaveChanges();
                }
            }
            else
            {
                TelegramBot.SendTextMessageAsync("Debes estar conectado para utilizar la aplicación por favor inicia sesión /LoginSesion", message.From.Id);
            }
            response = "Escriba /SudoWMenu para volver al menú de depósitos";
        }
    }
}
