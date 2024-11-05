using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public class OrderTracking : INotifyPropertyChanged
    {
        private int id;
        /// <summary>
        /// Id of Order
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
        /// list to keep track of a delivery
        /// </summary>
        private List<Tuple<DateTime?, string?>?>? tracking;
        public List<Tuple<DateTime?, string?>?>? Tracking
        {
            get { return tracking; }
            set
            {
                tracking = value;
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ID)));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tracking)));
            }
        }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
