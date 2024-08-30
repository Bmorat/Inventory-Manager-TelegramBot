using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Library
{
    /// <summary>
    /// Clase que gestiona la información de los usuarios y proveedores en el sistema.
    /// Esta clase sigue el patron SRP dado que delega la responsabilidad de admin de gestionar a los usuarios registrados mediante el uso de 
    /// archivos Json, sigue SRP porque la unica razon de existencia de esta clase es gestionar los usuarios registrados y las listas de espera/// 
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Lista de usuarios en espera de registro.
        /// </summary>
        public List<User> UserWaitlist = new List<User>();

        /// <summary>
        /// Lista de usuarios registrados en el sistema.
        /// </summary>
        public List<User> RegisteredUsers = new List<User>();

        /// <summary>
        /// Lista de proveedores en espera de registro.
        /// </summary>
        public List<Supplier> SupplierWaitlist = new List<Supplier>();

        /// <summary>
        /// Lista de proveedores registrados en el sistema.
        /// </summary>
        public List<Supplier> RegisteredSuppliers = new List<Supplier>();

        /// <summary>
        /// Ruta al archivo JSON que contiene la información de los usuarios.
        /// </summary>
        public string filePath;

        /// <summary>
        /// Cadena JSON que almacena la información de los usuarios.
        /// </summary>
        private string jsonStr;

        /// <summary>
        /// Ruta al segundo archivo JSON que contiene la información de los proveedores.
        /// </summary>
        public string filePath2;

        /// <summary>
        /// Cadena JSON que almacena la información de los proveedores.
        /// </summary>
        private string jsonStr2;

        /// <summary>
        /// Constructor de la clase UserManager.
        /// </summary>
        public UserManager()
        {
            filePath = @"../../docs/User.json";
            filePath2 = @"../../docs/Supplier.json";
            JsonToList();
        }

        /// <summary>
        /// Guarda los cambios realizados en la lista de usuarios y proveedores en los archivos JSON correspondientes.
        /// </summary>
        public void SaveChanges()
        {
            // Leer usuarios y proveedores existentes del archivo
            List<User> existingUsers = new List<User>();
            List<Supplier> existingSuppliers = new List<Supplier>();

            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
                existingUsers = JsonSerializer.Deserialize<List<User>>(jsonStr) ?? new List<User>();
            }

            if (File.Exists(filePath2))
            {
                jsonStr2 = File.ReadAllText(filePath2);
                existingSuppliers = JsonSerializer.Deserialize<List<Supplier>>(jsonStr2) ?? new List<Supplier>();
            }

            // Combinar usuarios existentes con los nuevos sin duplicar
            foreach (User user in RegisteredUsers)
            {
                if (!existingUsers.Any(x => x.ChatId == user.ChatId))
                {
                    existingUsers.Add(user);
                }
                else
                {
                    // Actualizar el usuario existente
                    int index = existingUsers.FindIndex(x => x.ChatId == user.ChatId);
                    if (index != -1)
                    {
                        existingUsers[index] = user;
                    }
                }
            }

            // Combinar proveedores existentes con los nuevos sin duplicar
            foreach (Supplier supplier in RegisteredSuppliers)
            {
                if (!existingSuppliers.Any(x => x.ChatId == supplier.ChatId))
                {
                    existingSuppliers.Add(supplier);
                }
                else
                {
                    // Actualizar el proveedor existente
                    int index = existingSuppliers.FindIndex(x => x.ChatId == supplier.ChatId);
                    if (index != -1)
                    {
                        existingSuppliers[index] = supplier;
                    }
                }
            }

            // Serializar y guardar la lista combinada en el archivo
            jsonStr = JsonSerializer.Serialize(existingUsers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonStr);

            jsonStr2 = JsonSerializer.Serialize(existingSuppliers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath2, jsonStr2);
        
            // Serializar y guardar la lista combinada en el archivo
            jsonStr = JsonSerializer.Serialize(existingUsers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonStr);
            // Serializar y guardar la lista combinada en el archivo
            jsonStr2 = JsonSerializer.Serialize(existingSuppliers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath2, jsonStr2);
        }

        /// <summary>
        /// Convierte la información almacenada en el archivo JSON de usuarios a listas de usuarios.
        /// </summary>
        public void JsonToList()
        {
            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
                RegisteredUsers = JsonSerializer.Deserialize<List<User>>(jsonStr) ?? new List<User>();
            }
            if (File.Exists(filePath2))
            {
                jsonStr2 = File.ReadAllText(filePath2);
                RegisteredSuppliers = JsonSerializer.Deserialize<List<Supplier>>(jsonStr2) ?? new List<Supplier>();
            }
        }

        /// <summary>
        /// Convierte la información almacenada en el archivo JSON de proveedores a listas de proveedores.
        /// </summary>
    
        /// <summary>
        /// Obtiene la lista de usuarios en espera de registro.
        /// </summary>
        /// <returns>Lista de usuarios en espera.</returns>
        public List<User> GetWaitingUsers()
        {
            return UserWaitlist;
        }

        /// <summary>
        /// Obtiene la lista de proveedores en espera de registro.
        /// </summary>
        /// <returns>Lista de proveedores en espera.</returns>
        public List<Supplier> GetWaitingSuppliers()
        {
            return SupplierWaitlist;
        }
    }
}
