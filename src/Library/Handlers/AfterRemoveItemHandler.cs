using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace Library
{
    /// <summary>
    /// Handler encargado de remover items de un depósito.
    /// </summary>
    public class AfterRemoveItemHandler : BaseHandler
    {
        private readonly WarehouseManager _manager = Singleton<WarehouseManager>.Instance;
        private Sells Vendido;
        private readonly Admin admin = new Admin();

        /// <summary>
        /// Constructor de la clase AfterRemoveItemHandler.
        /// </summary>
        /// <param name="next">El siguiente handler en la cadena.</param>
        public AfterRemoveItemHandler(BaseHandler next) : base(next)
        {
            Contract.Requires(next != null, "El siguiente handler no puede ser nulo.");
            Keywords = new[] { "/RemoveInW" };
        }

        /// <summary>
        /// Método que maneja el mensaje recibido.
        /// </summary>
        /// <param name="message">El mensaje recibido del usuario.</param>
        /// <param name="response">La respuesta que se enviará de vuelta al usuario.</param>
        protected override void InternalHandle(Message message, out string response)
        {
            try
            {
                Contract.Requires(message != null, "El mensaje no puede ser nulo.");
                Contract.Requires(!string.IsNullOrEmpty(message.Text), "El mensaje no puede estar vacío.");

                string patron = @"^/RemoveInW\s+(\S+)\s+(\d+)\s+(\d+)$";
                Match match = Regex.Match(message.Text, patron);

                if (!match.Success)
                {
                    throw new ArgumentException("El formato del mensaje es incorrecto. Debe ser: /RemoveInW Sección Id Cantidad.");
                }

                string name = match.Groups[1].Value;
                int id = Convert.ToInt32(match.Groups[2].Value);
                int quant = Convert.ToInt32(match.Groups[3].Value);

                name = StringExtensions.Capitalize(name);
                Warehouse warehouse = _manager.AuxiliarWarehouse;
                warehouse.JsonToList();
                Section section = warehouse.Sections.Find(x => x.Name == name);
                _manager.SellList();
                if (section == null)
                {
                    response = "Error: sección no encontrada";
                    Vendido = Sells.InvalidSells;
                    _manager.sellsave();
                    return;
                }

                if (!section.items.ContainsKey(id))
                {
                    response = "Error: item no encontrado en la sección";
                    Vendido = Sells.InvalidSells;
                    _manager.sellsave();
                    return;
                }

                if (section.items[id].Quantity < quant)
                {
                    TelegramBot.SendTextMessageAsync("Error: cantidad excedida").Wait();
                    Vendido = Sells.InvalidSells;
                    _manager.sellsave();
                    response = string.Empty;
                    return;
                }

                section.items[id].Quantity -= quant;
                Sells Venta = new Sells(section.items[id].Item.Name, section.items[id].Item.Category, quant);
                Vendido = Venta;
                warehouse.SaveChanges();

                int index = _manager.Warehouses.FindIndex(x => x.Name == warehouse.Name);
                if (index != -1)
                {
                    _manager.Warehouses[index] = warehouse;
                    _manager.sells.Add(Vendido);
                    _manager.sellsave();
                }

                response = "Item removido con éxito";
            }
            catch (Exception ex)
            {
                response = $"Se produjo un error al manejar el mensaje: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
