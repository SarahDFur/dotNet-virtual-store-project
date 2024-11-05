using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public enum Status { None, OrderConfirmed, OrderSent, ProvidedToCustomer }
    public class Order : INotifyPropertyChanged
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value; 
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Name of customer
        /// </summary>
        public string? CustomerName { get; set; }
        /// <summary>
        /// Customer email address
        /// </summary>
        public string? CustomerEmail { get; set; }
        /// <summary>
        /// Customer dwelling address
        /// </summary>
        public string? CustomerAddress { get; set; }
        /// <summary>
        /// The date the order was placed
        /// </summary>    
        public Status OrderStatus { get; set; }
        /// <summary>
        /// The date of payment completion 
        /// </summary>
        public DateTime? OrderDate { get; set; }
        /// <summary>
        /// Status of the order
        /// </summary>
        public DateTime? ShipDate { get; set; }
        /// <summary>
        /// Date customer recieved their order
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// List of items ordered
        /// </summary>
        public List<PO.OrderItem?>? Items { get; set; }
        /// <summary>
        /// The total price of the order
        /// </summary>
        public double TotalPrice { get; set; }
    }
}