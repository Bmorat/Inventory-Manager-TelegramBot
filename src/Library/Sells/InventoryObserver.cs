namespace Library
{
    /// <summary>
    /// Representa un observador para cambios en el inventario.
    /// </summary>
    public class InventoryObserver : IObserver
    {
        /// <summary>
        /// Actualiza al observador sobre cambios en el inventario. por ahora solamente nos manda un mesaje cuando hay una venta
        /// </summary>
        public void Update()
        {
            Console.WriteLine("Se ha producido una venta");
        }
    }
}
