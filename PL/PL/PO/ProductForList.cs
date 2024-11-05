using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public class ProductForList : INotifyPropertyChanged
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
        /// Name of product
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Name of artist
        /// </summary>
        public string? Artist { get; set; }
        /// <summary>
        /// Price of product
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Category of items
        /// </summary>
        public ArtStyles Style { get; set; }
        /// <summary>
        /// Image of a product
        /// </summary>
        public string? Image { get; set; }
    }
}
