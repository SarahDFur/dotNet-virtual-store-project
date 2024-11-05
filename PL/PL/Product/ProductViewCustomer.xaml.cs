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

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for ProductViewCustomer.xaml
    /// </summary>
    public partial class ProductViewCustomer : Window
    {
        public ProductViewCustomer(PO.ProductItem p)
        {
            InitializeComponent();
            WindowGrid.DataContext = p;
        }
    }
}
