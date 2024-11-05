using PL.Cart;
using PL.PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for CatalogForCustomerWindow.xaml
    /// </summary>
    public partial class CatalogForCustomerWindow : Window
    {
        public BlApi.IBl? bl = BlApi.Factory.GetBl();
        public BO.Cart boCart = new();
        ObservableCollection<PO.ProductItem> productItemsList = new();
        public CatalogForCustomerWindow()
        {
            InitializeComponent();
            productItemsList = Castings.ProductItem_ConvertIEnumerableToObservable(bl.Product.GetProductItemList());
            CatalogList.DataContext = productItemsList.OrderByDescending(x => x.Stocked);
        }

        private void CatalogList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductViewCustomer productViewCustomer = new((PO.ProductItem)CatalogList.SelectedItem);
            productViewCustomer.Show();
        }

        private void GoToCart_Click(object sender, RoutedEventArgs e)
        {
            CartWindow cartWindow = new CartWindow(boCart, this);
            cartWindow.Show();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem? item = FilterComboBox.SelectedItem as ComboBoxItem;
            switch (item?.Content!)
            {
                case "None":
                    CatalogList.ItemsSource = productItemsList;
                    break;
                case "Realism":
                    CatalogList.ItemsSource = productItemsList.Where(x => x.Style == PO.ArtStyles.Realism);
                    break;
                case "Cartoon":
                    CatalogList.ItemsSource = productItemsList.Where(x => x.Style == PO.ArtStyles.Cartoon);
                    break;
                case "Semi-Realism":
                    CatalogList.ItemsSource = productItemsList.Where(x => x.Style == PO.ArtStyles.SemiRealism);
                    break;
                case "Cubism":
                    CatalogList.ItemsSource = productItemsList.Where(x => x.Style == PO.ArtStyles.Cubism);
                    break;
                case "Abstract":
                    CatalogList.ItemsSource = productItemsList.Where(x => x.Style == PO.ArtStyles.Abstract);
                    break;
                }
        }

        private void addToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int v = ((sender as Button)!.DataContext as PO.ProductItem)!.ID;
                boCart = bl?.Cart.AddProductToCart(boCart, v)!;
            }
            catch(BO.ObjectStockOverflowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
             
        }
    }
}
