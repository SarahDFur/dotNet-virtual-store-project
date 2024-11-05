using System;
using System.Collections.Generic;
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
    /// Interaction logic for OrderTrackingWindow.xaml
    /// </summary>
    public partial class OrderTrackingWindow : Window
    {
        public BlApi.IBl? bl = BlApi.Factory.GetBl();
        PO.OrderTracking? trackingSelectedOrder = new();
        PO.Order? OrderForWindowOperations;
        //public enum Status { None, OrderConfirmed, OrderSent, ProvidedToCustomer }
        public OrderTrackingWindow()
        {
            InitializeComponent();
        }

        private static PO.OrderItem? BOTOPOOrderItem (BO.OrderItem? item_to_change)
        {
            return new PO.OrderItem()
            {
                ID = item_to_change!.IdOfOrderItem,
                ProductID = item_to_change.ProductID,
                ProductName = item_to_change.ProductName,
                Price = item_to_change.Price,
                Amount = item_to_change.Amount, 
                TotalPrice = item_to_change.TotalPrice,
                Image = item_to_change.Image
            };
        }

        private PO.OrderTracking? BOTOPOOrdertracking(BO.OrderTracking? orderTracking)
        {
            trackingSelectedOrder!.ID = orderTracking?.IdOfOrder ?? 0;
            switch (orderTracking?.OrderStatus)
            {
                case BO.Enums.Status.None:
                    trackingSelectedOrder.OrderStatus = PO.Status.None;
                    break;
                case BO.Enums.Status.OrderConfirmed:
                    trackingSelectedOrder.OrderStatus = PO.Status.OrderConfirmed;
                    break;
                case BO.Enums.Status.OrderSent:
                    trackingSelectedOrder.OrderStatus = PO.Status.OrderSent;
                    break;
                case BO.Enums.Status.ProvidedToCustomer:
                    trackingSelectedOrder.OrderStatus = PO.Status.ProvidedToCustomer;
                    break;
            }
            trackingSelectedOrder.Tracking = orderTracking?.Tracking;
            return trackingSelectedOrder;
        }

        private PO.Order? BOTOPOOrder(BO.Order? orderToChange)
        {
            OrderForWindowOperations = new()
            {
                ID = orderToChange?.IdOfOrder ?? 0,
                CustomerAddress = orderToChange?.CustomerAddress ?? "",
                CustomerEmail = orderToChange?.CustomerEmail ?? "",
                CustomerName = orderToChange?.CustomerName ?? "",
                OrderDate = orderToChange?.OrderDate ?? DateTime.MinValue,
                ShipDate = orderToChange?.ShipDate ?? DateTime.MinValue,
                DeliveryDate = orderToChange?.DeliveryDate ?? DateTime.MinValue,
                TotalPrice = orderToChange?.TotalPrice ?? 0,
                OrderStatus = (PO.Status)orderToChange!.OrderStatus,
            };
            OrderForWindowOperations.Items = new();
            foreach (BO.OrderItem? order_item in orderToChange.Items!)
            {
                OrderForWindowOperations?.Items?.Add(BOTOPOOrderItem(order_item));
            }
            return OrderForWindowOperations;
        }
        private void IDTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                int id = Convert.ToInt32((sender as TextBox)?.Text);
                BO.OrderTracking? helper_for_tracking;
                helper_for_tracking = bl?.Order.TrackOrder(id);
                trackingSelectedOrder = BOTOPOOrdertracking(helper_for_tracking);
                DataContext = trackingSelectedOrder;
            }
        }

        private void OrderViewBtn_Click(object sender, RoutedEventArgs e)
        {
            int id_of_order_to_send = Convert.ToInt32(OrderIDtxtbx?.Text);
            BO.Order? order_to_send_for_view = bl?.Order.GetOrder(id_of_order_to_send);
            //conversion need to check
            OrderOperationsWindow orderOperationsWindow = new(BOTOPOOrder(order_to_send_for_view));
            orderOperationsWindow.Show();
        }
    }
}
