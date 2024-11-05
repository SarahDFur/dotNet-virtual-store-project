using PL.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using PL.Order;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BlApi.IBl? bl = BlApi.Factory.GetBl();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainView_Click(object sender, RoutedEventArgs e)
        {
            ProductForList winProducts = new(bl!);
            winProducts.Show();
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            CatalogForCustomerWindow catalogForCustomerWindow = new();
            catalogForCustomerWindow.Show();

            //ProductViewCustomer productViewCustomer = new(new PO.Product()
            //{
            //    ID = 1,
            //    Title = "firstfirstfirstfirst fi rstfirstfirstfirstfirst",
            //    Artist = "artiat#1",
            //    Style = PO.ArtStyles.Realism,
            //    Price = 10.80,
            //    Image = "",
            //    Amount = 3
            //});
            //productViewCustomer.Show();
        }

        private void BtnTrackOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderTrackingWindow orderTrackingWindow = new();
            orderTrackingWindow.Show();
        }

        private void BtnTrackOrderSimulator_Click(object sender, RoutedEventArgs e)
        {
            TrackimgSimulator trackimgSimulator = new();
            trackimgSimulator.Show();
        }
    }
}
