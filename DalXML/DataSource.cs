using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// constractor
    /// </summary>
    static DataSource() { Initialize(); }
    /// <summary>
    /// initilize the lists of products, orders, and order-items
    /// </summary>
    static private void Initialize()
    {
        CreateProducts();
        CreateOrders();
        CreateOrderItems();
    }

    //static variable
    static readonly Random s_rand = new Random(DateTime.Now.Millisecond);

    //Data Lists
    internal static List<DO.Product?> DSProducts { get; } = new List<DO.Product?> { };
    internal static List<DO.Order?> DSOrders { get; } = new List<DO.Order?> { };
    internal static List<DO.OrderItem?> DSOrderItems { get; } = new List<DO.OrderItem?> { };

    /// <summary>
    /// create DSProudacts - list of products
    /// </summary>
    private static void CreateProducts()
    {
        string[] ProductsNames = new string[] { "Shrinking while drinking", "Blouse and pirates", "Serving clocks",
            "Light dream", "Vestage", "Skating fire", "Dress to impress", "Overall", "Sweater skull", "Coating dark" };
        string[] ArtistNames = new string[] { "Natasha", "Bessi", "Tray", "Noah", "Jack", "Eva", "Dianah", "Oliver", "Susannah", "Milly" };


        for (int i = 0; i < 10; i++)
        {
            DO.Product _product = new DO.Product
            {
                ID = Config.nextProductNumber,
                Name = ProductsNames[s_rand.Next(10)],
                Artist = ArtistNames[s_rand.Next(10)],
                Categories = (Enums.ArtStyles)s_rand.Next(1, 6), //cast random int into category
                Price = s_rand.Next(100) + 19.90, //Min price is 19.90
                InStock = s_rand.Next(30, 100),
                Image = "/image"+ s_rand.Next(1,5) + ".jpg",
                IsDeleted = false
            };
            if (i < 0.05 * 10) _product.InStock = 0;//about 5% of products are out of stock
            DSProducts.Add(_product);
        }

    }
    /// <summary>
    /// create DSOrders - list of orders
    /// </summary>
    private static void CreateOrders()
    {
        string[] Names = new string[] {"Avraham","Isaac","Jacob","Sarah","Rivka","Tal",
            "Rachel","Lea","Moshe","Aharon","David","Shlomo","Yael","Dvora","Tamar"};
        string[] Addresses = new string[] {"Jerusalem","Tel Aviv","Be'er Seva",
            "Heifa","Eylat","Bet Shemesh", "Ashdod","Ashkelon","Ra'anana","Dimona"};

        for (int i = 0; i < 20; i++)
        {
            string tmpName = Names[s_rand.Next(15)];
            DO.Order _order = new DO.Order
            {
                ID = Config.nextOrderNumber,
                CustomerName = tmpName,
                CustomerEmail = tmpName.ToLower() + "@gmail.com", //make email lowercase only
                CustomerAddress = Addresses[s_rand.Next(10)],
                OrderDate = DateTime.Now - new TimeSpan(s_rand.NextInt64(10L * 1000L * 1000L * 3600L * 24L * 10L)),
                ShipDate = null,
                DeliveryDate = null,
                IsDeleted = false
            };
            //about 80% of orders have been shipped
            if (i < 0.8 * 20) _order.ShipDate = _order.OrderDate + new TimeSpan(s_rand.NextInt64(10L * 1000L * 1000L * 3600L * 24L * 10L));
            //about 60% of shipped orders have been delivered
            if (i < 0.6 * 0.8 * 20) _order.DeliveryDate = _order.ShipDate + new TimeSpan(s_rand.NextInt64(10L * 1000L * 1000L * 3600L * 24L * 10L));

            DSOrders.Add(_order);
        }
    }
    /// <summary>
    /// create DSOrderItems - list of order-items
    /// </summary>
    private static void CreateOrderItems()
    {
        for (int i = 0; i < 20; i++)
        {
            int _orderId = s_rand.Next(Config.s_startOrderNumber, Config.s_startOrderNumber + DSOrders.Count);
            int numOfItems = s_rand.Next(1, 5);
            for (int j = 0; j < numOfItems; j++)
            {
                DO.Product? product = DSProducts[s_rand.Next(DSProducts.Count)]; //choose random product to put into the orderitems list
                DO.OrderItem _orderItem = new DO.OrderItem
                {
                    ID = Config.nextOrderItemNumber,
                    OrderID = _orderId,
                    ProductID = product?.ID ?? 0,
                    Price = product?.Price ?? 0,
                    Amount = s_rand.Next(1, 5),
                    Image = product?.Image,
                    IsDeleted = false
                };

                DSOrderItems.Add(_orderItem);
            }
        }
    }

    //config
    internal static class Config
    {
        //Order Items
        internal const int s_startOrderItemNumber = 0;
        private static int s_nextOrderItemNumber = s_startOrderItemNumber;
        internal static int nextOrderItemNumber { get => ++s_nextOrderItemNumber; }

        //Order
        internal const int s_startOrderNumber = 1000;
        private static int s_nextOrderNumber = s_startOrderNumber;
        internal static int nextOrderNumber { get => ++s_nextOrderNumber; }

        //Product
        internal const int s_startProductNumber = 111111;
        private static int s_nextProductNumber = s_startProductNumber;
        internal static int nextProductNumber { get => s_nextProductNumber += s_rand.Next(10, 100); }
    }
}
