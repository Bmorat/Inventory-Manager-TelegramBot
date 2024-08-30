using System.Security.Cryptography.X509Certificates;

namespace Library;

public class AppRunner //Clase encargada de correr la aplicacion
{

    public static void Main() //Metodo principal de la clase
    {
        Admin admin = new Admin(); //Instancia de la clase Admin
        Supplier supplier = new Supplier();     //Instancia de la clase Supplier
        Console.WriteLine("Bienvenido a EZ Inventory, para iniciar sesión escribe 1, para registrarte escribe 2");
        string answer = Console.ReadLine(); //Variable que almacena la respuesta del usuario si va hacer un login o un registro
        if (answer == "1")
        {
            Login();
        }
        else if (answer == "2")
        {
            register();
        }
        else
        {
            Console.WriteLine("Opción no válida");
        }

        void Login()    //Metodo encargado de realizar el login
        {
            try
            {
                Console.WriteLine("Por favor escribe tu usuario");
                string user = Console.ReadLine();
                Console.WriteLine("Por favor escribe tu contraseña");
                string pass = Console.ReadLine();
                admin.UserManager.JsonToList(); //Carga la lista de usuarios registrados en el archivo.Json
                Usuarios FoundUser = admin.UserManager.RegisteredUsers.Find(x => x.User == user && x.Password == pass); //Busca el usuario en la lista de usuarios registrados
                if (FoundUser != null) //Si el usuario y contraseña son correctos
                {
                    Console.WriteLine("Bienvenido " + FoundUser.Name);
                    FoundUser.logged = true;
                    
                    if (FoundUser.admin)
                    {
                        AppRunner.menuAdmin(); //Menu de administrador
                    }
                    else //pruebas de que todo funciona
                    {
                        //Menu de los MANAGERS
                        WarehouseManager warehouseManager = new WarehouseManager();
                        warehouseManager.WarehouseMenu();
                        //Menu de los Supplier
                        

                    }
                }
                else
                {
                    Console.WriteLine("Usuario o contraseña incorrectos");
                }
            }
            catch (NullReferenceException) //Excepcion en caso de que no se encuentre el usuario
            {
                Console.WriteLine("Ocurrió un error ");
                
            }
        }
        
        void register () //Metodo encargado de realizar el registro
        {
            Console.WriteLine("Por favor escribe tu nombre");
            string name = Console.ReadLine();
            Console.WriteLine("Por favor escribe tu usuario");
            string user = Console.ReadLine();
            Console.WriteLine("Por favor escribe tu contraseña");
            string pass = Console.ReadLine();
            User usuario = new User(name);
            usuario.User = user;
            usuario.Password = pass;
            usuario.admin = false;
            usuario.logged = false;
            admin.UserManager.UserWaitlist.Add(usuario);
            Console.WriteLine("Usuario ingresado con exito, espera a que un administrador lo apruebe");
            
        }
    }

    public static void menuAdmin() //Menu de administrador
    {
        Admin admin = new Admin();
        Console.WriteLine("Bienvenido administrador, que deseas hacer?");
        Console.WriteLine("1. Agregar usuarios");
        Console.WriteLine("2. Agregar proveedores");
        Console.WriteLine("3. Administrar Almacen");
        Console.WriteLine("4. Buscar producto");
        Console.WriteLine("5. Ver ventas");
        string answer = Console.ReadLine();
        switch (answer) //Switch para elegir la opcion deseada
        {
            case "1": //Agregar usuarios
                List<User> WaitingUsers = admin.UserManager.GetWaitingUsers(); //Lista de usuarios en espera
                Console.WriteLine("Usuarios en espera:");
                foreach (User usuario in WaitingUsers)
                {
                    Console.WriteLine(usuario.Name);
                }
                Console.WriteLine("Escribe el nombre del usuario que deseas aprobar");
                string name = Console.ReadLine();
                User ApproveUser = WaitingUsers.Find(x => x.Name == name); 
                if (ApproveUser != null)
                {
                    admin.AddUser(ApproveUser); //Agrega el usuario a la lista de usuarios registrados
                    Console.WriteLine("Usuario aprobado con éxito");
                    admin.UserManager.SaveChanges(); //Guarda los cambios en el archivo.Json
                }
                else
                {
                    Console.WriteLine("Usuario no encontrado");
                }
                break;
            case "2": //Agregar proveedores
                List<Supplier> WaitingSuppliers = admin.UserManager.GetWaitingSuppliers(); //Lista de proveedores en espera
                Console.WriteLine("Proveedores en espera:");
                foreach (Supplier proveedor in WaitingSuppliers)
                {
                    Console.WriteLine(proveedor.Name);
                }
                Console.WriteLine("Escribe el nombre del usuario que deseas aprobar");
                string name2 = Console.ReadLine();
                Supplier ApproveSupplier = WaitingSuppliers.Find(x => x.Name == name2); //Busca el proveedor en la lista de proveedores en espera
                if (ApproveSupplier != null) //Verifica si el proveedor no esta vacia la lista
                {
                    admin.AddSupplier(ApproveSupplier);
                    Console.WriteLine("Proveedor aprobado con éxito");
                    admin.UserManager.SaveChanges(); //Guarda los cambios en el archivo.Json
                }
                else
                {
                    Console.WriteLine("Proveedor no encontrado");
                }
                break;

            case "3": //Nuevo deposito
                admin.NewWarehouse();
                AppRunner.menuAdmin(); //Menu de administrador
                break;
            case "4": //Buscar producto
                admin.SearchProduct();
                AppRunner.menuAdmin();
                break;
            case "5": //Ver ventas
                admin.ViewSales();
                AppRunner.menuAdmin();
                break;
        }
    }
}
