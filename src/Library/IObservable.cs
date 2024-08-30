using System;

namespace Library
{
    /// <summary>
    /// Representa una interfaz para los objetos observables en el patrón Observer.
    /// Un objeto observable mantiene una lista de sus observadores y les notifica de cualquier cambio.
    /// </summary>
    public interface IObservable //Patron de diseño Observer
    {
        /// <summary>
        /// Suscribe un observador para recibir notificaciones de cambios.
        /// Este método se utiliza para agregar un observador a la lista de observadores.
        /// </summary>
        /// <param name="observer">El observador que se va a suscribir.</param>
        void Subscribe(IObserver observer); //Metodo para suscribir un observador

        /// <summary>
        /// Notifica a todos los observadores suscritos de un cambio.
        /// Este método se llama cuando el objeto observable experimenta un cambio,
        /// lo que provoca que todos los observadores sean notificados.
        /// </summary>
        void NotifyObservers(); //Metodo para notificar a los observadores,
    }
}
