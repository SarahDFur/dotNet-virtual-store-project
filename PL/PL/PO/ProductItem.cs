using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public class ProductItem : INotifyPropertyChanged
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
        /// Category of items
        /// </summary>
        public ArtStyles Style { get; set; }
        /// <summary>
        /// Price of product
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// If the product *is* in stock
        /// </summary>
        public bool Stocked { get; set; }
        /// <summary>
        /// Amount of a product in the cart
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// link to file containing art piece
        /// </summary>
        public string? Image { get; set; }
    }
}
