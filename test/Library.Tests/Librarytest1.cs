using System.Reflection;

namespace Library.Tests;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;

public class Tests
{
    [Test]
    public void ItemConstructor()
    {
        string name = "TestItem";
        Item item = new Item(name);
        Assert.AreEqual(name, item.Name);
    }
    [Test]
    public void ApproveSupplier()
    {
        Admin admin = new Admin();
        Supplier Nicolas = new Supplier();
        admin.UserManager.SupplierWaitlist.Add(Nicolas);
       admin.UserManager.RegisteredSuppliers.Add(Nicolas);
        List<Supplier> lista = new List<Supplier>();
        lista.Add(Nicolas);
        Assert.AreEqual(admin.UserManager.RegisteredSuppliers, lista);

    }
        
    [Test]
    public void Item_Category_SetCorrectly()
    {
        Item item = new Item("TestItem");
        string category = "TestCategory";
        item.Category = category;
        Assert.AreEqual(category, item.Category);
    }

    [Test]
    public void StockItemConstructor()
    {
        int id = 1;
        Item item = new Item("TestItem");
        int quantity = 10;
        StockItem stockItem = new StockItem(id, item, quantity);
        Assert.AreEqual(id, stockItem.Id);
        Assert.AreSame(item, stockItem.Item);
        Assert.AreEqual(quantity, stockItem.Quantity);
    }

    [Test]
    public void StockItemCantidad()
    {
        StockItem stockItem = new StockItem(1, new Item("TestItem"), 10);
        int newQuantity = 20;
        stockItem.Quantity = newQuantity;
        Assert.AreEqual(newQuantity, stockItem.Quantity);
    }
    [Test]
    public void SectionAddStockItem()
    {
        Section section = new Section("TestSection");
        Item item = new Item("TestItem");
        StockItem stockItem = new StockItem(1, item, 10);
        section.items.Add(stockItem.Id, stockItem);
        Assert.IsTrue(section.items.ContainsKey(stockItem.Id));
        Assert.AreSame(stockItem, section.items[stockItem.Id]);
    }

    [Test]
    public void SectionRemoveStockItem()
    {
        Section section = new Section("TestSection");
        Item item = new Item("TestItem");
        StockItem stockItem = new StockItem(1, item, 10);
        section.items.Add(stockItem.Id, stockItem);
        section.items.Remove(stockItem.Id);
        Assert.IsFalse(section.items.ContainsKey(stockItem.Id));
    }

    [Test]
    public void AddsItemToSection()
    {
        Warehouse warehouse= new Warehouse("TestWareHouse");
        Section section = new Section("TestCategory");
        warehouse.AddSection(section);
        using (StringReader sr = new StringReader("TestItem\nTestCategory\n10\n"))
        {
            Console.SetIn(sr);
            warehouse.AgregarItem();
        }
        Assert.That(section.items.Count, Is.EqualTo(1));
        Assert.AreEqual("TestItem", section.items.Values.First().Item.Name);
        Assert.AreEqual(10, section.items.Values.First().Quantity);
    }

    [Test]
    public void RemovesItemFromSection()
    {
        Warehouse warehouse= new Warehouse("");
        Section section = new Section("TestCategory");
        Item item = new Item("TestItem");
        StockItem stockItem = new StockItem(1, item, 10);
        section.items.Add(stockItem.Id, stockItem);
        warehouse.AddSection(section);
        using (StringReader sr = new StringReader("TestCategory\n1\n5\n"))
        {
            Console.SetIn(sr);
            warehouse.RemoverItem();
        }
        Assert.That(section.items.Count, Is.EqualTo(1));
        Assert.AreEqual(5, section.items.Values.First().Quantity);
    }
    [Test]
    public void CreateSupplier()
    {
        Supplier Juan = new Supplier("Juan");
        string nombre = "Juan";
        int id = 0;

        Assert.AreEqual(Juan.Name, nombre);
        Assert.AreEqual(Juan.id, id);
        Assert.AreEqual(Juan.logged, false);
        Assert.AreEqual(Juan.admin, false);
        Assert.AreEqual(Juan.Password, null);
        Assert.AreEqual(Juan.User, null);


    }
     
    [Test]
    public void NewWarehouse()
    {
        string name = "Central";
        Warehouse warehouse = new Warehouse("Central");
        Assert.AreEqual(warehouse.Name, name);     
    }

    [Test]
    public void NewSection()
    {
        Warehouse warehouse = new Warehouse("Central");
        Section section = new Section("Books");

        warehouse.AddSection(section);

        Assert.AreEqual(1, warehouse.Sections.Count);
        Assert.AreEqual("Books", warehouse.Sections[0].Name);
    }

    [Test]
    public void NewCategory()
    {
        Warehouse warehouse = new Warehouse("Central");
        List<Section> Sections = new List<Section>();
        string ProductName = "TestItem";
        Item item = new Item(ProductName);
        string category = "TestCategory";
        Section section = Sections.FirstOrDefault(s => s.Name == category);
        if (section == null)
        {
            section = new Section(category);
            Sections.Add(section);
        }
        int IdProduct = section.items.Count(); 

        Assert.IsFalse(warehouse.Sections.Any(s => s.Name == category));
        Assert.That(Sections.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddSection()
    {
        Warehouse warehouse = new Warehouse("Central");
        Section section = new Section("Books");

        warehouse.AddSection(section);

        Assert.AreEqual("Books", warehouse.Sections[0].Name);
    }

    [Test]
    public void AddItem()
    {
        WarehouseManager manager = new WarehouseManager();
        string itemName = "Papas";

        Warehouse warehouseFound = manager.Warehouses.FirstOrDefault(x => x.Name == itemName);
        if (warehouseFound == null)
        {
            Warehouse warehouse = new Warehouse(itemName);
            manager.Warehouses.Add(warehouse);
        }

        Assert.IsTrue(manager.Warehouses.Any(x => x.Name == itemName));
    }

    [Test]
    public void SearchItem()
    {
        Warehouse warehouse = new Warehouse("Central");
        Section section = new Section("TestCategory");
        Item Item = new Item("TestItem");
        StockItem stockItem = new StockItem(1, Item, 10);
        section.items.Add(stockItem.Id, stockItem);
        warehouse.Sections.Add(section);
        Warehouse result= null;
        foreach (var Section in warehouse.Sections)
        {
            if (section.items.ContainsKey(1) && section.items[1].Quantity> 0);
            {
                result = warehouse;
                break;
            }
        }
    Assert.IsNotNull(result);
    Assert.AreEqual("Central", result.Name);


    }

    [Test]
    public void BuyItem()
    {
        WarehouseManager manager = new WarehouseManager();
        Warehouse warehouse = new Warehouse("Central");
        Section section = new Section("comida");
        Item item = new Item("papas");
        StockItem stockItem = new StockItem(0, item, 5);
        section.items.Add(stockItem.Id, stockItem);
        warehouse.AddSection(section);
        Assert.IsTrue(warehouse.Sections.Contains(section));
        Assert.AreEqual(1, section.items.Count);
        Assert.AreEqual("papas", section.items.Values.First().Item.Name);
        Assert.AreEqual(5, section.items.Values.First().Quantity);

        
    }


    [Test]
    public void SearchStock()
    {
    
    Warehouse warehouse1 = new Warehouse("Central");
    Section section = new Section("TestCategory");
    Item item = new Item("TestItem");
    StockItem stockItem = new StockItem(1, item, 10);
    section.items.Add(stockItem.Id, stockItem);
    warehouse1.Sections.Add(section);
    Warehouse result = null;
    foreach (var s in warehouse1.Sections)
    {
        if (s.items.ContainsKey(1) && s.items[1].Quantity > 0)
        {
            result = warehouse1;
            break;
        }
    }
    Assert.IsNotNull(result);
    Assert.AreEqual(warehouse1, result);

    }
    
    [Test]
    public void MissingStock()
    {
            Warehouse warehouse = new Warehouse("TestWarehouse");
            Section section = new Section("TestSection");
            warehouse.Sections.Add(section);

            Item item1 = new Item("Item1");
            StockItem stockItem1 = new StockItem(1, item1, 10);
            section.items.Add(stockItem1.Id, stockItem1);

            Item item2 = new Item("Item2");
            StockItem stockItem2 = new StockItem(2, item2, 0);
            section.items.Add(stockItem2.Id, stockItem2);

            Item item3 = new Item("Item3");
            StockItem stockItem3 = new StockItem(3, item3, 5);
            section.items.Add(stockItem3.Id, stockItem3);

            var missingStockItems = warehouse.MissingStock();
            Assert.AreEqual(1, missingStockItems.Count);
            Assert.AreEqual(stockItem2, missingStockItems[0]);
    }
    
    [Test]
    public void ViewDaySales()
    {
        List<Section> sections = new List<Section>();
        WarehouseManager manager = new WarehouseManager();
        Section section = new Section("TestSection");
        sections.Add(section);

        Item item = new Item("TestItem");
        StockItem stockItem = new StockItem(1, item, 10);
        section.items.Add(stockItem.Id, stockItem);

        int id = 1;
        int quantity = 5;
        if (section.items.ContainsKey(id))
        {
            if (section.items[id].Quantity >= quantity)
            {
                section.items[id].Quantity -= quantity;
                Sells sale = new Sells(section.items[id].Item.Name, section.items[id].Item.Category, quantity);
                
                Assert.AreEqual("TestItem", sale.Product);
                Assert.AreEqual(5, sale.Quantity);
                Assert.AreEqual(1, stockItem.Id);
                manager.SellViewer();
            }
        }
    } 

    [TestFixture]
    public class HandlerTests
    {
       
        private IHandler _firstHandler;

        [SetUp]
        public void Setup()
        {
           TelegramBotClient bot = new TelegramBotClient("7353367298:AAFOA7_Ee6OXlRwSlXy-Xb5tTvhFNnUPayw"); // Token del Bot de telegram obtenido mediante BotFather

            LocationApiClient client = new LocationApiClient();
            IAddressFinder finder = new AddressFinder(client);

            _firstHandler = 
            new HelloHandler(bot,
            new IniciarSesionHandler(
            new LoginUsuarioHandler(bot,
            new SuppOrUserHandler(bot,
            new UsuariosPreRegistroHandler(
            new ExitHandler(
            new SupplierPreRegistroHandler(
            new SupplierRegistroHandler(
            new AdminAddSupplierHandler(
            new UsuarioRegistroHandler(
            new MenuDeSupplierHandler(
            new WareHouseManagerHandler(
            new EveryShowWarehousesHandler(
            new SearchPreItemIdHandler(
            new SearchItemIdHandler( client,
            new PreShowInventoryHandler(
            new AddItemHandler2(
            new AfterRemoveItemHandler(
            new AdminAddUserHandler(
            new PreShowInventoryHandler(
            new PreAddItemHandler(
            new AddItemHandler(
            new SellViewerHandler(
            new MenuDeUserHandler(
            new PreRemoveItemHandler(
            new RemoveItemHandler(
            new ShowInventoryHandler(
            new MenudeAdminHandler(
            new AdminUserPreRegistroHandler(
            new PreAddWarehouseHandler(
            new AddWarehouseHandler(finder, bot,
            new PreItemQuantityHandler(
            new ItemQuantityHandler(null) ))
            ))))))))))))))))))))))))))))));
        }
        
        public Admin admin = new Admin();

    
        [Test]
        public void WareHouseManagerTest()
        {
            // Arrange
            var message = new Message
            {

                Text = "/SudoWMenu"
            };
            string expectedResponse = "---------------------------------------------------------------------------------------------\n" +
                       "\nBienvenido a la Sección de depósitos. ¿Qué deseas hacer?\n" +
                       "\n1_Mostrar Depósitos  📦                   ➡️  /SudoShowW\n" +
                       "\n2_Agregar Depósitos    📦                  ➡️  /SudoAddwh\n" +
                       "\n3_Agregar Artículo        📥                  ➡️  /SudoAddItem\n" +
                       "\n4_Remover Artículo (venta)     📤     ➡️  /SudoRemoveItem\n" +
                       "\n5_Ver inventarios      🗂                      ➡️  /SudoShowInventory\n" +
                       "\n6_Ver ventas            💵                        ➡️  /SudoSellViewer\n" +
                       "\n7_Volver al menú principal   📖       ➡️  /SudoMenu\n" +
                       "\n---------------------------------------------------------------------------------------------";

            // Act
            string response;
            IHandler firstHandler = new WareHouseManagerHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void UsuariosPreRegistroHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/Users"
            };
            string expectedResponse = "🤖 Hola Usuario, a continuación necesito que ingreses /RegUser Nombre Usuario Contraseña\nEjemplo /RegUser botname botuser botpass\n";

            // Act
            string response;
            IHandler firstHandler = new UsuariosPreRegistroHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }


        [Test]
        public void SupplierPreRegistroHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/Proveedores"
            };

            string expectedResponse = "Se produjo un error al manejar el mensaje";


            // Act
            string response;
            IHandler firstHandler = new SupplierPreRegistroHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
        [Test]
        public void SupplierRegistroHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SuppReg"
            };
            string expectedResponse = "No puedo reconocer ese Comando por favor intenta de nuevo";

            // Act
            string response;
            IHandler firstHandler = new SupplierPreRegistroHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
        
        [Test]
        public void PreShowInventoryHandlerTest()
        { 
            WarehouseManager _manager;
            // Arrange
            var message = new Message
            {
                Text = "/SudoShowInventory",
                From = new User { Id = 12345 }
            };
           
            string expectedResponse = "Error: usuario no encontrado.";
            // Act
            string response;
            // IHandler firstHadler = PreShowInventoryHandler(null);
            IHandler firstHandler = new PreShowInventoryHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));

        }

        }
 

        [Test]
        public void SeachPreItemIdHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoSearchItemId"
            };
            string expectedResponse = "No puedo reconocer ese Comando por favor intenta de nuevo";

            // Act
            string response;
            IHandler firstHandler = new SearchPreItemIdHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
        [Test]
        public void SearchItemIdHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SearchID 1"
            };
            string expectedResponse = "Artículo no encontrado.";

            // Act
            string response;
            IHandler firstHandler = new SearchItemIdHandler(null, null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
    
        [Test]
        public void PreRemoveItemHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoRemoveItem"
            };
            string expectedResponse = "No puedo ejecutar este comando ya que no estás loggeado como un usuario o administrador. Por favor escribe \"/LoginSession\" para continuar.";

            // Act
            string response;
            IHandler firstHandler = new PreRemoveItemHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void PreAddWarehouseHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoAddW"
            };
            string expectedResponse = "Escriba /AddWh e ingrese el nombre del depósito y la dirección del mismo. Ejemplo: /AddWh Central 8 de octubre y estero bellaco";

            // Act
            string response;
            IHandler firstHandler = new PreAddWarehouseHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void PreAddItemHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoAddItem"
            };
            string expectedResponse = "Debes estar conectado para utilizar la aplicación. Por favor, inicia sesión con /LoginSesion";

            // Act
            string response;
            IHandler firstHandler = new PreAddItemHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void WareHouseManagerHandlerTest()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoWMenu"
            };
            string expectedResponse = 
                        "---------------------------------------------------------------------------------------------\n" +
                        "\nBienvenido a la Sección de depósitos. ¿Qué deseas hacer?\n" +
                        "\n1_Mostrar Depósitos  📦                   ➡️  /SudoShowW\n" +
                        "\n2_Agregar Depósitos    📦                  ➡️  /SudoAddwh\n" +
                        "\n3_Agregar Artículo        📥                  ➡️  /SudoAddItem\n" +
                        "\n4_Remover Artículo (venta)     📤     ➡️  /SudoRemoveItem\n" +
                        "\n5_Ver inventarios      🗂                      ➡️  /SudoShowInventory\n" +
                        "\n6_Ver ventas            💵                        ➡️  /SudoSellViewer\n" +
                        "\n7_Volver al menú principal   📖       ➡️  /SudoMenu\n" +
                        "\n---------------------------------------------------------------------------------------------";


            // Act
            string response;
            IHandler firstHandler = new WareHouseManagerHandler(null);
            firstHandler.Handle(message, out response);

            // Assert
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
        
        [Test]
        public void IniciarSesionHandler()
        {
            // Arrange
            var message = new Message
            {
                Text = "/LoginSesion"
            };
            string expectedResponse = "---------------------------------------------------------------------------------------------\n" +
                       "\n🤖 Por favor ingrese su usuario y contraseña escriba '/Login'\n" +
                       "\nseguido de su nombre de usuario 🙋🏻‍♂️ y seguido su contraseña\n" +
                       "\nEjemplo: /Login botuser botpassword 😎\n" +
                       "\n---------------------------------------------------------------------------------------------\n" +
                       "\n➡️     /Login + Usuario + Contraseña";

            // Act
            string response;
            IHandler firstHandler = new IniciarSesionHandler(null);
            firstHandler.Handle(message, out response);  

            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void  MenuDeAdminHandler()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoMenu"
            };
            string expectedResponse = "No es posible ejecutar este comando ya que no estás loggeado en tu cuenta o tu cuenta no está registrada como administrador.";
            string response;
            IHandler firstHandler = new MenudeAdminHandler(null);
            firstHandler.Handle(message, out response);  

            Assert.That(response, Is.EqualTo(expectedResponse));
        }
    
        [Test]
        public void  MenuDeSupplierHandler()
        {
            // Arrange
            var message = new Message
            {
                Text = "/MenuSupplier"
            };
            string expectedResponse = "---------------------------------------------------------------------------------------------\n" +
                       "\nBienvenido Proveedor, ¿qué deseas realizar en la Sección Almacenes?\n" +
                       "\n1. Mostrar Depósitos  📦             ➡️  /SudoShowW\n" +
                       "\n2. Buscar el stock de un artículo ➡️  /SearchItemID\n" +
                       "\n3. Para cerrar sesión 🚪               ➡️  /Exit\n" +
                       "\n---------------------------------------------------------------------------------------------";
            string response ;
            IHandler firstHandler = new MenuDeSupplierHandler(null);
            firstHandler.Handle(message, out response);  


            Assert.That(response, Is.EqualTo(expectedResponse));
        }
    
        [Test] // preguntar
        public void AddItemHandler()
        {
            // Arrange
            var message = new Message
            {
                Text = "/AddItemDeposito"
            };
            string resp="Debes estar conectado para utilizar la aplicación. Por favor inicia sesión con /LoginSesion";

            IHandler firstHandler = new AddItemHandler(new AddItemHandler2(null));
            firstHandler.Handle(message, out string response);    

            Assert.That(response, Is.EqualTo(resp));

        }

        [Test] //FALTA PREGUNTAR
        public void AddWarehouseHandlerTest()

        {
            // Arrange
            var message = new Message
            {
                Text = "/AddWh NuevoDeposito"
            };
            string expectedResponse = "Error: debes proporcionar un nombre y una dirección para el almacén.";
            string response;
            LocationApiClient client = new LocationApiClient();
            IAddressFinder finder = new AddressFinder(client); 
            TelegramBotClient bot = new TelegramBotClient("7353367298:AAFOA7_Ee6OXlRwSlXy-Xb5tTvhFNnUPayw");
            // Assert
            IHandler firstHandler = new AddWarehouseHandler(finder, bot, null);
            firstHandler.Handle(message, out response);     
            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void AdminAddUserHandler()
        {
            // Arrange
            var message = new Message
            {
                Text = "/SudoUserRegister NombreUsuario ContraseñaUsuario"
            };
            string expectedResponse = "Usuario no encontrado en la lista de espera";
            string response;
            
            // Assert
            IHandler firstHandler = new AdminAddUserHandler(null);
            firstHandler.Handle(message, out response); 

            Assert.That(response, Is.EqualTo(expectedResponse));
        }
                [Test]
        public void  PreItemQuantityHandler()//deberia devovlernos el manejo del siguiente handler
        {
            // Arrange
            var message = new Message
            {
                Text = "/WItemQuantity"
            };
            string expectedResponse = "🤖: Ingrese /ItemQuantity + el nombre del deposito en el cual desea obtener la cantidad de items\nEjemplo: /ItemQuantity Central";
            string response ;
            IHandler firstHandler = new PreItemQuantityHandler(null);
            firstHandler.Handle(message, out response);  


            Assert.That(response, Is.EqualTo(expectedResponse));
        }
        [Test]
        public void  ItemQuantityHandler()//al no estar registrado como usuario deberia devolvernos un error
        {
            // Arrange
            Message message = new Message
            {
                Text = "/ItemQuantity Central",
               
            };
            string expectedResponse = "Error Usuario no encontrado";
            string response ;
            IHandler firstHandler = new ItemQuantityHandler(null);
            firstHandler.Handle(message, out response);  

            Assert.That(response, Is.EqualTo(expectedResponse));
        }

}



