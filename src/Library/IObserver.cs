using System;

namespace Library
{
    /// <summary>
    /// Representa una interfaz para los observadores en el patrón Observer.
    /// Los observadores son notificados de los cambios en el objeto observable.
    /// </summary>
    public interface IObserver //Interfaz para los observadores
    {
        /// <summary>
        /// Método que se llama cuando el objeto observable experimenta un cambio.
        /// Implementa la lógica de reacción al cambio en el objeto observable.
        /// </summary>
        void Update();
    }
}
