using BO;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        public BlApi.IBl? bl = BlApi.Factory.GetBl();
        ObservableCollection<PO.OrderItem> OrderItemsList = new();
        private PO.Cart myCart = new();
        CatalogForCustomerWindow lastWindow;

        public CartWindow(BO.Cart cart, CatalogForCustomerWindow catalogWindow)
        {
            InitializeComponent();
            myCart = BO_To_PO_cartCasting(cart);
            CartDetails.DataContext = myCart;
            ItemsListView.DataContext = myCart.Items;
            lastWindow = catalogWindow;
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int newOrderID = (int)bl?.Cart.CartPayment(PO_To_BO_cartCasting(myCart))!;
                MessageBox.Show("Thank for your purchase!\n Your order number is " + newOrderID);
                this.Close();
            }
            catch(BO.FormatIsIncorrectException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(BO.ObjectStockOverflowException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch(BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(BO.DoubleFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BackToCatalog_Click(object sender, RoutedEventArgs e)
        {
            lastWindow.boCart = PO_To_BO_cartCasting(myCart);
            this.Close();
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            int productID = ((sender as Button)!.DataContext as PO.OrderItem)!.ProductID;
            int productAmount = ((sender as Button)!.DataContext as PO.OrderItem)!.Amount;
            try
            {
                BO.Cart helper = bl?.Cart.UpdateAmountOfProductInCart(PO_To_BO_cartCasting(myCart), productID, productAmount + 1)!;
                myCart = BO_To_PO_cartCasting(helper);
                CartDetails.DataContext = myCart;
                ItemsListView.DataContext = myCart.Items;
            }
            catch(BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.FormatIsIncorrectException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.ObjectStockOverflowException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            int productID = ((sender as Button)!.DataContext as PO.OrderItem)!.ProductID;
            int productAmount = ((sender as Button)!.DataContext as PO.OrderItem)!.Amount;
            try
            {
                BO.Cart helper = bl?.Cart.UpdateAmountOfProductInCart(PO_To_BO_cartCasting(myCart), productID, productAmount - 1)!;
                myCart = BO_To_PO_cartCasting(helper);
                CartDetails.DataContext = myCart;
                ItemsListView.DataContext = myCart.Items;
            }
            catch (BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.FormatIsIncorrectException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.ObjectStockOverflowException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int productID = ((sender as Button)!.DataContext as PO.OrderItem)!.ProductID;
            int productAmount = ((sender as Button)!.DataContext as PO.OrderItem)!.Amount;
            try
            {
                BO.Cart helper = bl?.Cart.UpdateAmountOfProductInCart(PO_To_BO_cartCasting(myCart), productID, 0)!;
                myCart = BO_To_PO_cartCasting(helper);
                CartDetails.DataContext = myCart;
                ItemsListView.DataContext = myCart.Items;
            }
            catch (BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.FormatIsIncorrectException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.ObjectStockOverflowException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public static BO.Cart PO_To_BO_cartCasting(PO.Cart cart)
        {
            BO.Cart newCart;

            if (cart.Items != null)
            {
                var list = from oi in cart.Items
                           select new BO.OrderItem()
                           {
                               IdOfOrderItem= oi.ID,
                               ProductID = oi.ProductID,
                               ProductName= oi.ProductName,
                               Price= oi.Price,
                               Amount= oi.Amount,
                               TotalPrice = oi.TotalPrice,
                               Image= oi.Image                            
                           };
                newCart = new BO.Cart()
                {
                    CustomerName = cart.Name,
                    CustomerEmail = cart.Email,
                    CustomerAddress = cart.Address,
                    items = list.ToList(),
                    TotalPrice = cart.TotalPrice
                };
            }
            else
            {
                newCart = new BO.Cart()
                {
                    CustomerName = cart.Name,
                    CustomerEmail = cart.Email,
                    CustomerAddress = cart.Address,
                    TotalPrice = cart.TotalPrice
                };
            }

            return newCart;
        }
        public static PO.Cart BO_To_PO_cartCasting(BO.Cart cart)
        {
            PO.Cart newCart;

            if (cart.items != null)
            {
                var list = from oi in cart.items
                           select new PO.OrderItem()
                           {
                               ID=oi.IdOfOrderItem,
                               ProductID = oi.ProductID,
                               ProductName= oi.ProductName,
                               Price= oi.Price,
                               Amount= oi.Amount,
                               TotalPrice = oi.TotalPrice,
                               Image= oi.Image
                           };
                newCart = new PO.Cart()
                {
                    Name = cart.CustomerName!,
                    Email=cart.CustomerEmail!,
                    Address = cart.CustomerAddress!,
                    Items = list.ToList(),
                    TotalPrice = cart.TotalPrice
                };
            }
            else
            {
                newCart = new PO.Cart()
                {
                    Name = cart.CustomerName!,
                    Email = cart.CustomerEmail!,
                    Address = cart.CustomerAddress!,
                    TotalPrice = cart.TotalPrice
                };
            }

            return newCart;
        }


    }
}
