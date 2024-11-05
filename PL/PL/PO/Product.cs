using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public enum ArtStyles { None, Realism, Cartoon, SemiRealism, Cubism, Abstract }
    public class Product : INotifyPropertyChanged
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
        /// title of art piece
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// name of artist
        /// </summary>
        public string? Artist { get; set; }
        /// <summary>
        /// style of art piece
        /// </summary>
        public ArtStyles? Style { get; set; }
        /// <summary>
        /// price of art piece
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// amout in stock
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// link to file containing art piece
        /// </summary>
        public string? Image { get; set; }
    }
}
