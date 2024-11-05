using BlApi;
using BlImplementation;
using BO;

namespace BlTest;

internal class Program
{
    static BlApi.IBl? accessor = BlApi.Factory.GetBl();
    static void Main(string[] args)
    {
        string choice;
        do
        {
            Console.WriteLine($@"MAIN MENU: choose entity to test - 
a - product
b - order
c - cart
x - exit
");
            choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "a":
                    proudactTests();
                    break;
                case "b":
                    orderTests();
                    break;
                case "c":
                    cartTests();
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

    private static void proudactTests()
    {
        string choice;
        int intInput;
        bool formatFlag;
        Product product;
        Product? productN;

        do
        {
            Console.WriteLine($@"PRODUCT MENU: choose test - 
a - add product
b - delete product
c - update product
d - get all products
e - get a product for manager
f - get a product for customer
x - back to MAIN MENU
");
            choice = Console.ReadLine() ?? "";
            try
            {
                switch (choice)
                {
                    case "a": //add
                        product = getUserProduct();
                        accessor!.Product.Add(product);
                        break;
                    
                    case "b": //delete
                        
                        Console.Write("enter product id to delete: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while(!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter product id to delete: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        accessor!.Product.Delete(intInput);
                        break;
                    
                    case "c": //update
                        
                        Console.Write("enter product id to update: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter product id to update: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        productN = accessor!.Product.GetProductByIdForManager(intInput);
                        Console.WriteLine(productN);
                        
                        Product updatedProduct = getUserProduct(intInput);
                        //compare user product to existing product
                        updatedProduct.NameOfProduct = (updatedProduct.NameOfProduct != "") ? updatedProduct.NameOfProduct : productN?.NameOfProduct;
                        updatedProduct.Artist = (updatedProduct.Artist != "") ? updatedProduct.Artist : productN?.Artist;
                        updatedProduct.Categories = (updatedProduct.Categories != Enums.ArtStyles.None) ? updatedProduct.Categories : productN!.Categories;
                        updatedProduct.Price = ((updatedProduct.Price != productN?.Price) && (updatedProduct.Price > 0)) ? updatedProduct.Price : productN!.Price;
                        updatedProduct.AmountInStock = (updatedProduct.AmountInStock != productN?.AmountInStock)? updatedProduct.AmountInStock : productN.AmountInStock;
                        updatedProduct.Image = (updatedProduct.Image != "") ? updatedProduct.Image : productN?.Image;
                        
                        accessor.Product.Update(updatedProduct);
                        break;
                    
                    case "d": //get all
                        IEnumerable<ProductForList?> allProduct = accessor!.Product.GetProductList();
                        foreach (ProductForList? item in allProduct)
                            Console.WriteLine(item);
                        break;
                    
                    case "e": //get for manager
                        
                        Console.Write("manager, enter product id to search: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nmanager, enter product id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        productN = accessor!.Product.GetProductByIdForManager(intInput);
                        Console.WriteLine(productN);
                        break;
                    
                    case "f": //get for customer
                        
                        Console.Write("customer, enter product id to search: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\ncustomer, enter product id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }

                        int randAmount = new Random().Next(4);
                        Cart cartForTesting = new Cart()
                        {
                            CustomerName = "Moshe",
                            CustomerEmail = "moshe@gmail.com",
                            CustomerAddress = "Jerusalem",
                            items = new List<OrderItem?>()
                            {
                                new OrderItem()
                                {
                                    IdOfOrderItem = 123,
                                    ProductID = intInput,
                                    ProductName = "aaa",
                                    Price = 29.90,
                                    Amount = randAmount,
                                    TotalPrice = 29.90 * randAmount
                                },
                                new OrderItem()
                                {
                                    IdOfOrderItem = 124,
                                    ProductID = 987654,
                                    ProductName = "bbb",
                                    Price = 15.50,
                                    Amount = 2,
                                    TotalPrice = 15.50 * 2
                                }
                            },
                            TotalPrice = 15.50 * 2 + 29.90 * randAmount
                        };

                        ProductItem? productItem = accessor!.Product.GetProductByIdForCustomer(intInput, cartForTesting);
                        Console.WriteLine(productItem);
                        break;
                    
                    case "x":
                        break;
                    
                    default:
                        Console.WriteLine("Choice does not exist");
                        break;
                }
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (FormatIsIncorrectException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (ObjectStockOverflowException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (DoubleFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (CouldNotDeleteObjectException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }

        } while (choice != "x");
    }

    private static void cartTests()
    {
        string choice;
        int intInput;
        int intMoreInput;
        bool formatFlag;

        Console.WriteLine("CART MANU:\n");
        Cart? cart = new Cart();

        do
        {
            Console.WriteLine($@"CART MENU: choose test - 
a - add product to cart
b - update amount of product in cart
c - approve cart and payment
x - back to MAIN MENU
");
            choice = Console.ReadLine() ?? "";
            try
            {
                switch (choice)
                {
                    case "a": //add product to cart
                        
                        Console.Write("enter product id to add to cart: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter product id to add to cart: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        cart = accessor!.Cart.AddProductToCart(cart!, intInput);
                        Console.WriteLine(cart);
                        break;
                    
                    case "b": //update amount of product in cart
                        
                        Console.Write("enter product id to update amount in cart: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter product id  to update amount in cart: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        Console.Write("enter new amount: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intMoreInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter now amount: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        cart = accessor!.Cart.UpdateAmountOfProductInCart(cart!, intInput, intMoreInput);
                        Console.WriteLine(cart);
                        break;
                    
                    case "c": //approve cart and payment
                        cart = getBuyerInfo(cart);
                        accessor!.Cart.CartPayment(cart!);
                        Console.WriteLine(cart);
                        break;
                    
                    case "x":
                        break;
                    
                    default:
                        Console.WriteLine("Choice does not exist");
                        break;
                }
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (ObjectStockOverflowException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (DoubleFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (FormatIsIncorrectException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }




        } while (choice != "x");
    }

    private static void orderTests()
    {
        string choice;
        int intInput, intInput2, intInput3;
        bool formatFlag;
        Order? order;

        do
        {
            Console.WriteLine($@"ORDER MENU: choose test - 
a - get an order
b - get all orders
c - update shipment date
d - update delivery date
e - track order
f - update order
x - back to MAIN MENU
");
            choice = Console.ReadLine() ?? "";
            try
            {
                switch (choice)
                {
                    case "a": //get an order
                        
                        Console.Write("enter order id to search: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter order id to search: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        order = accessor!.Order.GetOrder(intInput);
                        Console.WriteLine(order);
                        break;
                    
                    case "b": //get all orders
                        IEnumerable<OrderForList?> allOrders =  accessor!.Order.GetOrders();
                        foreach (OrderForList? ord in allOrders)
                            Console.WriteLine(ord);
                        break;
                    
                    case "c": //update ship date
                        
                        Console.Write("enter order id to update shipment date: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag){
                            Console.Write("incorrect format, only digits allowed\nenter order id to update shipment date: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        order =  accessor!.Order.UpdateShipDate(intInput);
                        Console.WriteLine(order);
                        break;
                    
                    case "d": //update delivery date
                        
                        Console.Write("enter order id to update deleivery date: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter order id to update delivery date: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        order = accessor!.Order.UpdateDeliveryDate(intInput);
                        Console.WriteLine(order);
                        break;
                    
                    case "e": //track order
                        
                        Console.Write("enter order id to track: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag) {
                            Console.Write("incorrect format, only digits allowed\nenter order id to track: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }
                        
                        OrderTracking? track = accessor!.Order.TrackOrder(intInput);
                        Console.WriteLine(track);
                        break;
                    
                    case "f": //update order

                        Console.Write("enter order id to update: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        while (!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter order id to update: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput);
                        }

                        Console.Write("enter product id to update in order: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput2);
                        while (!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter product id to update in order: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput2);
                        }

                        Console.Write("enter new/updated amount of product in order: ");
                        formatFlag = int.TryParse(Console.ReadLine(), out intInput3);
                        while (!formatFlag)
                        {
                            Console.WriteLine("incorrect format, only digits allowed\nenter new/updated amount of product in order: ");
                            formatFlag = int.TryParse(Console.ReadLine(), out intInput3);
                        }

                        order = accessor!.Order.UpdateOrder(intInput, intInput2, intInput3);
                        Console.WriteLine(order);

                        //Console.WriteLine("Method not implemented yet");
                        break;
                    
                    case "x":
                        break;
                    
                    default:
                        Console.WriteLine("Choice does not exist");
                        break;
                }
            }
            catch (ObjectNotFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (DatesNotChronologicalException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (DoubleFoundException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (FormatIsIncorrectException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }
            catch (ObjectStockOverflowException ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.GetType() + ": " + ex.InnerException.Message);
            }

        } while(choice!="x");
    }

    private static Product getUserProduct(int id = 0)
    {
        bool formatFlag;
        Product userProduct = new Product();
        Console.WriteLine(@"enter new data for PRODUCT or press enter to skip
(fields with * are required)");
        
        switch(id)
        {
            case 0://add new
                Console.Write("* ID: ");
                int productID;
                formatFlag = int.TryParse(Console.ReadLine(), out productID);
                while (!formatFlag)
                {
                    Console.Write("REQUIRED FIELD\n* ID: ");
                    formatFlag = int.TryParse(Console.ReadLine(), out productID);
                }
                userProduct.ID = productID;
                break;
            
            default://update existing
                Console.WriteLine("ID: " + id);
                userProduct.ID = id;
                break;
        }        

        Console.Write("new name: ");
        userProduct.NameOfProduct = Console.ReadLine() ?? "";
        
        Console.Write("new artist name: ");
        userProduct.Artist = Console.ReadLine() ?? "";

        Console.Write(@"new category - enter a number or enter to skip: 
    1 - {0}
    2 - {1}
    3 - {2}
    4 - {3}
    5 - {4} 
",
            (Enums.ArtStyles)1, (Enums.ArtStyles)2, (Enums.ArtStyles)3, (Enums.ArtStyles)4, (Enums.ArtStyles)5);
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

        Console.Write("new amount in stock: ");
        int productStock;
        int.TryParse(Console.ReadLine(), out productStock);
        userProduct.AmountInStock = productStock;
        
        Console.Write("new image's link: ");
        userProduct.Image = Console.ReadLine() ?? "";

        return userProduct;
    }

    private static Cart? getBuyerInfo(Cart? c)
    {
        if (c != null)
        {
            Console.WriteLine("enter your info");

            Console.WriteLine("Name:");
            c.CustomerName = Console.ReadLine() ?? "";
            Console.WriteLine("Email address:");
            c.CustomerEmail = Console.ReadLine() ?? "";
            Console.WriteLine("Home address:");
            c.CustomerAddress = Console.ReadLine() ?? "";
        }
        return c;
    }

}

