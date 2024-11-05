using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public class OrderForList : INotifyPropertyChanged
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
        private string? customerName;
        public string? CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomerName)));
            }
        }
        /// <summary>
        /// Status of the order
        /// </summary>
        private Status orderStatus;
        public Status OrderStatus
        {
            get { return orderStatus; }
            set
            {
                orderStatus = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderStatus)));
            }
        }
        /// <summary>
        /// Amount of items in order
        /// </summary>
        private int amount;
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
            }
        }
        /// <summary>
        /// Total price of orders
        /// </summary>
        private double totalPrice;
        public double TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(totalPrice)));
            }
        }
    }
}
