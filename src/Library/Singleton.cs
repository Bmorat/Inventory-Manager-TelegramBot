using System;

namespace Library
{
    /// <summary>
    /// Proporciona una implementación genérica del patrón Singleton.
    /// Este patrón asegura que una clase tenga solo una instancia y proporciona un punto de acceso global a esa instancia.(refactoring guru)
    /// </summary>
    /// <typeparam name="T">El tipo de la clase Singleton. Debe tener un constructor público sin parámetros.</typeparam>
    public class Singleton<T> where T : new() //Clase Singleton
    {
        /// <summary>
        /// La única instancia de la clase Singleton.
        /// </summary>
        private static T instance; //Instancia de la clase Singleton

        /// <summary>
        /// Obtiene la instancia única de la clase Singleton.
        /// Si la instancia no existe, se crea una nueva.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}


