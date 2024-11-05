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

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderOperationsWindow.xaml
    /// </summary>
    public partial class OrderOperationsWindow : Window
    {
        public BlApi.IBl? bl = BlApi.Factory.GetBl();
        ObservableCollection<PO.Order?>? forOrders = new();
        PO.Order? ord;
        ObservableCollection<PO.OrderItem?>? forItem = new();

        #region Constructors
        public OrderOperationsWindow(PO.OrderForList? order = null)
        {
            InitializeComponent();
            if (order != null)
            {
                ord = UpdateOrderInfo(order);
                DataContext = ord;
                forItem = Castings.Orderitem_Items_ConvertIEnumerableToObservable(ord?.Items!);
            }
            //OrderView.ItemsSource = forOrders;
        }
        public OrderOperationsWindow(PO.Order? order = null)
        {
            InitializeComponent();
            if (order != null)
            {
                DataContext = order;
            }
        }

        private PO.Order? UpdateOrderInfo(PO.OrderForList? orderForList)
        {
            try
            {
                if (orderForList != null)
                {
                    BO.Order? order_for_display = bl?.Order.GetOrder(orderForList.ID);
                    //IEnumerable<PO.OrderForList?>? order = forLists?.Where(x => x?.ID == order_for_display?.IdOfOrder);
                    ord = new()
                    {
                        ID = order_for_display!.IdOfOrder,
                        CustomerName = order_for_display?.CustomerName ?? "",
                        CustomerAddress = order_for_display?.CustomerAddress ?? "",
                        CustomerEmail = order_for_display?.CustomerEmail ?? "",
                        OrderStatus = (PO.Status)order_for_display!.OrderStatus,
                        OrderDate = order_for_display?.OrderDate ?? DateTime.MinValue,
                        ShipDate = order_for_display?.ShipDate ?? DateTime.MinValue,
                        DeliveryDate = order_for_display?.DeliveryDate ?? DateTime.MinValue,
                        TotalPrice = order_for_display?.TotalPrice ?? 0
                    };
                    ///////////////////////////////////////////////
                    List<PO.OrderItem?> order_items_list_helper = new();
                    foreach (BO.OrderItem? orderItem in order_for_display!.Items!)
                    {
                        order_items_list_helper.Add(new PO.OrderItem()
                        {
                            ID = orderItem?.IdOfOrderItem ?? 0,
                            ProductID = orderItem?.ProductID ?? 0,
                            ProductName = orderItem?.ProductName ?? "",
                            Price = orderItem?.Price ?? 0,
                            Amount = orderItem?.Amount ?? 0,
                            TotalPrice = orderItem?.TotalPrice ?? 0,
                            Image = orderItem?.Image
                        });

                    }
                    //ord.Items = (List<PO.OrderItem?>?)o;
                    ord.Items = new(order_items_list_helper);
                    forOrders?.Add(ord);

                    return ord;
                }
                else
                    return null;
                    //forOrders = null;
            }
            catch (BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }   
        #endregion
        
        #region Change infromation     
        private void ChangeStatus_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch((sender as TextBox)?.Text)
                {
                    case "None":
                        ord!.OrderStatus = PO.Status.None; break;
                    case "OrderConfirmed":
                        ord!.OrderStatus = PO.Status.OrderConfirmed; break;
                    case "OrderSent":
                        ord!.OrderStatus = PO.Status.OrderSent; break;
                    case "ProvidedToCustomer":
                        ord!.OrderStatus = PO.Status.ProvidedToCustomer; break;
                }
            }
        }    
        #endregion                           
        
        private void ShipOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Order helper = bl?.Order.UpdateShipDate(((sender as Button)!.DataContext as PO.Order)!.ID)!;
                DataContext = BO_To_PO_OrderCasting(helper);
            }
            catch (BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.DoubleFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void DeliveryOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Order helper = bl?.Order.UpdateDeliveryDate(((sender as Button)!.DataContext as PO.Order)!.ID)!;
                DataContext = BO_To_PO_OrderCasting(helper);
            }
            catch (BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (BO.DoubleFoundException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch(BO.DatesNotChronologicalException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        #region Casting functions
        public static PO.Order BO_To_PO_OrderCasting(BO.Order order)
        {
            PO.Order newOrder;
            if(order.Items!=null)
            {
                var list = from oi in order.Items
                           select new PO.OrderItem()
                           {
                               ID = oi.IdOfOrderItem,
                               ProductID = oi.ProductID,
                               ProductName = oi.ProductName,
                               Price = oi.Price,
                               Amount = oi.Amount,
                               TotalPrice = oi.TotalPrice,
                               Image = oi.Image
                           };
                newOrder = new()
                {
                    ID = order.IdOfOrder,
                    CustomerName = order.CustomerName,
                    CustomerAddress = order.CustomerAddress,
                    CustomerEmail = order.CustomerEmail,
                    OrderStatus = (PO.Status)order.OrderStatus,
                    OrderDate = order.OrderDate,
                    ShipDate = order.ShipDate,
                    DeliveryDate = order.DeliveryDate,
                    TotalPrice = order.TotalPrice,
                    Items = list.ToList()
                };
            }
            else
            {
                newOrder = new()
                {
                    ID = order.IdOfOrder,
                    CustomerName = order.CustomerName,
                    CustomerAddress = order.CustomerAddress,
                    CustomerEmail = order.CustomerEmail,
                    OrderStatus = (PO.Status)order.OrderStatus,
                    OrderDate = order.OrderDate,
                    ShipDate = order.ShipDate,
                    DeliveryDate = order.DeliveryDate,
                    TotalPrice = order.TotalPrice,
                };
            }
            return newOrder;
        }
        public static BO.Order PO_To_BO_OrderCasting(PO.Order order)
        {
            BO.Order newOrder;
            if(order.Items!=null)
            {
                var list = from oi in order.Items
                           select new BO.OrderItem()
                           {
                               IdOfOrderItem = oi.ID,
                               ProductID = oi.ProductID,
                               ProductName = oi.ProductName,
                               Price = oi.Price,
                               Amount = oi.Amount,
                               TotalPrice = oi.TotalPrice,
                               Image = oi.Image
                           };
                newOrder = new()
                {
                    IdOfOrder = order.ID,
                    CustomerName = order.CustomerName,
                    CustomerAddress = order.CustomerAddress,
                    CustomerEmail = order.CustomerEmail,
                    OrderStatus = (BO.Enums.Status)order.OrderStatus,
                    OrderDate = order.OrderDate,
                    ShipDate = order.ShipDate,
                    DeliveryDate = order.DeliveryDate,
                    TotalPrice = order.TotalPrice,
                    Items = list.ToList()
                };
            }
            else
            {
                newOrder = new()
                {
                    IdOfOrder = order.ID,
                    CustomerName = order.CustomerName,
                    CustomerAddress = order.CustomerAddress,
                    CustomerEmail = order.CustomerEmail,
                    OrderStatus = (BO.Enums.Status)order.OrderStatus,
                    OrderDate = order.OrderDate,
                    ShipDate = order.ShipDate,
                    DeliveryDate = order.DeliveryDate,
                    TotalPrice = order.TotalPrice,
                };
            }
            return newOrder;
        }
        #endregion
    }
}
