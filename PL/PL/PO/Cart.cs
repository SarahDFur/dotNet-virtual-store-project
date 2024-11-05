using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public class Cart : INotifyPropertyChanged
    {              
        public event PropertyChangedEventHandler? PropertyChanged;  
        /// <summary>
        /// Name of customer
        /// </summary>
        private string? name; //need to change
        public string Name
        {
            get { return name!; }
            set
            {
                name = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
       
        
        /// <summary>
        /// Customer email address
        /// </summary>     
        private string? email; //need to change
        public string Email
        {
            get { return email!; }
            set
            {
                email = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }
               
        /// <summary>
        /// Customer dwelling address
        /// </summary>
        private string? address; 
        public string Address
        {
            get { return address!; }
            set
            {
                address = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
            }
        }

        /// <summary>
        /// List of items ordered
        /// </summary>
        public List<OrderItem?>? Items { get; set; }
        /// <summary>
        /// Total price of everything we wish to buy (collective price of all items ordered)
        /// </summary>
        public double TotalPrice { get; set; }
    }
}