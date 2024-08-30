using System.Net.Sockets;
using Telegram.Bot.Requests;

namespace Library
{
    /// <summary>
    /// Representa una superclase abstracta usuarios que define los atributos que todos los usuarios van a heredar
    /// </summary>
    public abstract class Usuarios
    {
        /// <summary>
        /// Obtiene o establece el nombre del usuario.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece el ID del usuario.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario utilizado para iniciar sesión.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el usuario está conectado.
        /// </summary>
        public bool logged { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el usuario tiene privilegios de administrador.
        /// </summary>
        public bool admin { get; set; }

        public long ChatId {get; set;}

        public double Latitude{get; set;}

        public double Longitude{get;set;}
    }
}
