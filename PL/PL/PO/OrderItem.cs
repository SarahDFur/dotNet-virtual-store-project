using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public class OrderItem : INotifyPropertyChanged
    {
        private int id;
        /// <summary>
        /// ID number of an item in an order
        /// </summary>     
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
        /// ID number of product
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Name of product with ID no. ^
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// Price of one product
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Amount of product ordered
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// (Price of a product) * (Amount we ordered) 
        /// </summary>
        public double TotalPrice { get; set; }
        /// <summary>
        /// Icon of item in an order
        /// </summary>
        public string? Image { get; set; }
    }
}