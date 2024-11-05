using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DalApi;
using DO;
namespace DalTest;

internal class Program
{
    static DalApi.IDal? accessor = DalApi.Factory.Get();
    
    static void Main(string[] args)
    {

        IEnumerable<DO.OrderItem?>? d = accessor?.OrderItem.GetAllItemsInOrder(1001);
        d?.ToString();

        string choice;

        do
        {
            Console.WriteLine($@"MAIN MENU: choose entity to test - 
a - products
b - orders
c - items in order
x - exit
");
            choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "a":
                    productsTests();
                    break;

                case "b":
                    ordersTests();
                    break;

                case "c":
                    orderItemsTests();
                    break;

                case "x":
                    Console.WriteLine("goodbye");
                    break;

                default:
                    Console.WriteLine("Choice does not exist");
                    break;
            }
        } while (choice != "x");
    }

    private static void productsTests()
    {
        string choice;
        int intInput;
        Product product;
        bool formatFlag;

        do
        {
            Console.WriteLine($@"PRODUCT MENU: choose test - 
a - add new product
b - get a product
c - get all products
d - update existing product
e - delete product
x - back to MAIN MENU
");            
            choice = Console.ReadLine() ?? "";

            try
            {
                switch (choice)
                {
                    case "a"://add object
                        product = getUserProduct();
                        accessor!.Product.Add(product);
                        break;

                    case "b"://print object with "id"
                        Console.Write("enter product id to search: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter product id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        product = accessor!.Product.GetById(intInput);
                        Console.WriteLine(product);
                        break;

                    case "c"://print the list of objects
                        IEnumerable<Product?> allProducts = accessor!.Product.GetAll();
                        foreach (Product? p in allProducts)
                            Console.WriteLine(p);
                        break;

                    case "d"://update object
                        Console.Write("enter product id to update: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter product id to update: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        product = accessor!.Product.GetById(intInput);
                        Console.WriteLine(product);

                        Product userProduct = getUserProduct();
                        //compare user product to existing product
                        userProduct.ID = intInput;
                        userProduct.Name = (userProduct.Name != "") ? userProduct.Name : product.Name;
                        userProduct.Artist = (userProduct.Artist != "") ? userProduct.Artist : product.Artist;
                        userProduct.Categories = (userProduct.Categories != Enums.ArtStyles.None) ? userProduct.Categories : product.Categories;
                        userProduct.Price = (userProduct.Price > 0) ? userProduct.Price : product.Price;
                        userProduct.InStock = (userProduct.InStock > 0) ? userProduct.InStock : product.InStock;
                        userProduct.Image = (userProduct.Image != "") ? userProduct.Image : product.Image;
                        
                        //call update
                        accessor!.Product.Update(userProduct);
                        break;

                    case "e"://delete object
                        
                        Console.Write("enter product id to delete: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter product id to delete: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        accessor!.Product.Delete(intInput);
                        break;

                    case "x":
                        break;

                    default:
                        Console.WriteLine("Choice not in range. Enter a correct letter");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (choice != "x");
    }

    private static void ordersTests()
    {
        int intInput;
        Order order;
        string choice;
        bool formatFlag;

        do
        {
            Console.WriteLine($@"ORDER MENU: choose test - 
a - add new order
b - get an order
c - get all orders
d - update existing order
e - delete order
x - back to MAIN MENU
");
            choice = Console.ReadLine() ?? "";

            try
            {
                switch (choice)
                {
                    case "a"://add object
                        order = getUserOrder();
                        accessor!.Order.Add(order);
                        break;

                    case "b"://print object with "id"
                        Console.Write("enter order id to search: ");
                         formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        order = accessor!.Order.GetById(intInput);
                        Console.WriteLine(order);
                        break;

                    case "c"://print the list of objects
                        IEnumerable<Order?> allOrders = accessor!.Order.GetAll();
                        foreach (Order? o in allOrders)
                            Console.WriteLine(o);
                        break;

                    case "d"://update object
                        Console.Write("enter order id to update: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order id to update: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        order = accessor!.Order.GetById(intInput);
                        Console.WriteLine(order);
                        
                        Order userOrder = getUserOrder();
                        //compare user order to existing order
                        userOrder.ID = intInput;
                        userOrder.CustomerName = (userOrder.CustomerName != "") ? userOrder.CustomerName : order.CustomerName;
                        userOrder.CustomerEmail = (userOrder.CustomerEmail != "") ? userOrder.CustomerEmail : order.CustomerEmail;
                        userOrder.CustomerAddress = (userOrder.CustomerAddress != "") ? userOrder.CustomerAddress : order.CustomerAddress;
                        userOrder.OrderDate = userOrder.OrderDate ?? order.OrderDate;
                        userOrder.ShipDate = userOrder.ShipDate ?? order.ShipDate;
                        userOrder.DeliveryDate = userOrder.DeliveryDate ?? order.DeliveryDate;
                        
                        //call update
                        accessor!.Order.Update(userOrder);
                        break;

                    case "e"://delete object
                        Console.Write("enter order id to delete: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order id to delete: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        accessor!.Order.Delete(intInput);
                        break;

                    default:
                        Console.WriteLine("Choice not in range. Enter a correct letter");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while(choice!= "x");
    }

    private static void orderItemsTests()
    {
        int intInput, intMoreInput;
        OrderItem orderItem;
        string choice;
        bool formatFlag;

        do
        {
            Console.WriteLine($@"ORDER-ITEM MENU: choose test - 
a - add new order item
b - get an order item by ID number
c - get an order item by order ID and product ID
d - get all order items in an existing order
e - get all order items
f - update existing order item
g - delete order item
x - back to MAIN MENU
");
            choice = Console.ReadLine() ?? "";

            try
            {
                switch (choice)
                {
                    case "a"://add object
                        orderItem = getUserOrderItem();
                        accessor!.OrderItem.Add(orderItem);
                        break;

                    case "b"://print object with "id"
                        Console.Write("enter order item id to search: ");
                         formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order item id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        orderItem = accessor!.OrderItem.GetById(intInput);
                        Console.WriteLine(orderItem);
                        break;

                    case "c"://print object with "order num" and "product num"
                        Console.Write("enter order id to search: ");
                         formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        Console.Write($"enter product id to search in order {intInput}: ");
                         formatFlag = int.TryParse(Console.ReadLine(), out intMoreInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter product id to search in order: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        orderItem = accessor!.OrderItem.GetItem(intInput, intMoreInput);
                        Console.WriteLine(orderItem);
                        break;

                    case "d"://print the list of objects belonging specific order
                        Console.Write("enter order id to print all items: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order id to print all items: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        IEnumerable<OrderItem?> allItemsInOrder = accessor!.OrderItem.GetAllItemsInOrder(intInput);
                        foreach (OrderItem? oi in allItemsInOrder)
                            Console.WriteLine(oi);
                        break;

                    case "e"://print the list of objects
                        IEnumerable<OrderItem?> allOrderItems = accessor!.OrderItem.GetAll();
                        foreach (OrderItem? oi in allOrderItems)
                            Console.WriteLine(oi);
                        break;

                    case "f"://update object
                        Console.Write("enter order item id to update: ");
                         formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order item id to update: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        orderItem = accessor!.OrderItem.GetById(intInput);
                        Console.WriteLine(orderItem);
                        
                        OrderItem userOrderItem = getUserOrderItem();
                        //compare user order to existing order
                        userOrderItem.ID = intInput;
                        userOrderItem.ProductID = (userOrderItem.ProductID != 0) ? userOrderItem.ProductID : orderItem.ProductID;
                        userOrderItem.OrderID = (userOrderItem.OrderID != 0) ? userOrderItem.OrderID : orderItem.OrderID;
                        userOrderItem.Price = (userOrderItem.Price != 0) ? userOrderItem.Price : orderItem.Price;
                        userOrderItem.Amount = (userOrderItem.Amount != 0) ? userOrderItem.Amount : orderItem.Amount;
                        userOrderItem.Image = (userOrderItem.Image != "") ? userOrderItem.Image : orderItem.Image;
                        
                        //call update
                        accessor!.OrderItem.Update(userOrderItem);
                        break;

                    case "g"://delete object
                        Console.Write("enter order item id to delete: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order item id to delete: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        accessor!.OrderItem.Delete(intInput);
                        break;

                    default:
                        Console.WriteLine("Choice not in range. Enter a correct letter");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while(choice!= "x");
    }

    private static Product getUserProduct()
    {
        
        Product userProduct = new Product();

        Console.WriteLine(@"enter new data or press enter to skip
(fields with * are required)");

        Console.Write("* ID: ");
        int productID;
        bool formatFlag = int.TryParse(Console.ReadLine(), out productID);
        while (!formatFlag)
        {
            Console.Write("REQUIRED FIELD\n* ID: ");
            formatFlag = int.TryParse(Console.ReadLine(), out productID);
        }
        userProduct.ID = productID;

        Console.Write("new name: ");
        userProduct.Name = Console.ReadLine() ?? "";
        
        Console.Write("new artist's name: ");
        userProduct.Artist = Console.ReadLine() ?? "";

        Console.Write(@"new category - enter a number or enter to skip: 
    1 - {0}
    2 - {1}
    3 - {2}
    4 - {3}
    5 - {4} 
", (Enums.ArtStyles)1, (Enums.ArtStyles)2, (Enums.ArtStyles)3, (Enums.ArtStyles)4, (Enums.ArtStyles)5);
        
        int productCategory;
        int.TryParse(Console.ReadLine(), out productCategory);
        switch (productCategory)
        {
            case 1: 
                userProduct.Categories = Enums.ArtStyles.Realism; 
                break;
            case 2:
                userProduct.Categories = Enums.ArtStyles.Cartoon;
                break;
            case 3: 
                userProduct.Categories = Enums.ArtStyles.SemiRealism;
                break;
            case 4: 
                userProduct.Categories = Enums.ArtStyles.Cubism; 
                break;
            case 5: 
                userProduct.Categories = Enums.ArtStyles.Abstract; 
                break;
            default:
                userProduct.Categories = Enums.ArtStyles.None;
                break;
        }
        
        Console.Write("new price: ");
        double productPrice;
        double.TryParse(Console.ReadLine(), out productPrice);
        userProduct.Price = productPrice;        
        
        Console.Write("new stock: ");
        int productStock;
        int.TryParse(Console.ReadLine(), out productStock);
        userProduct.InStock = productStock;

        Console.Write("new image's link: ");
        userProduct.Image = Console.ReadLine() ?? "";

        userProduct.IsDeleted = false;

        return userProduct;
    }

    private static Order getUserOrder()
    {

        Order userOrder = new Order();

        Console.WriteLine("enter new data or press enter to skip");

        Console.Write("new customer name: ");
        userOrder.CustomerName = Console.ReadLine() ?? "";
        
        Console.Write("new customer email: ");
        userOrder.CustomerEmail = Console.ReadLine() ?? "";
        
        Console.Write("new customer address: ");
        userOrder.CustomerAddress = Console.ReadLine() ?? "";

        Console.Write("new order date: ");
        DateTime.TryParse(Console.ReadLine(), out DateTime od);
        userOrder.OrderDate = od;

        Console.Write("new ship date: ");
        DateTime.TryParse(Console.ReadLine(), out DateTime sd);
        userOrder.ShipDate = sd;

        Console.Write("new delivery date: ");
        DateTime.TryParse(Console.ReadLine(), out DateTime dd);
        userOrder.DeliveryDate = dd;

        userOrder.IsDeleted = false;

        return userOrder;
    }

    private static OrderItem getUserOrderItem()
    {

        OrderItem userOrderItem = new OrderItem();

        Console.WriteLine("enter new data or press enter to skip");

        Console.Write("new product ID: ");
        int productID;
        int.TryParse(Console.ReadLine(), out productID);
        userOrderItem.ProductID = productID;

        Console.Write("new order ID: ");
        int orderID;
        int.TryParse(Console.ReadLine(), out orderID);
        userOrderItem.OrderID = orderID;

        Console.Write("new price: ");
        double itemPrice;
        double.TryParse(Console.ReadLine(), out itemPrice);
        userOrderItem.Price = itemPrice;

        Console.Write("new amount: ");
        int itemAmount;
        int.TryParse(Console.ReadLine(), out itemAmount);
        userOrderItem.Amount = itemAmount;

        Console.Write("new image's link: ");
        userOrderItem.Image = Console.ReadLine() ?? "";

        userOrderItem.IsDeleted = false;

        return userOrderItem;
    }
}

