using BlApi;
using BO;
using PL.Order;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        public BlApi.IBl? bl1 = BlApi.Factory.GetBl();
        ObservableCollection<PO.ProductForList?>? productForList = new();
        ObservableCollection<PO.OrderForList?>? orderForList = new();
        public InventoryPage(BlApi.IBl? bl = null)
        {
            InitializeComponent();
            if(bl is not null) //create window for first use
            {
                productForList = Castings.ProductForList_ConvertIEnumerableToObservable(bl1.Product.GetProductList());
                orderForList = Castings.OrderForList_ConvertIEnumerableToObservable(bl1.Order.GetOrders());
            }
            else //create window with updated products
            {
                productForList = Castings.ProductForList_ConvertIEnumerableToObservable(bl!.Product.GetProductList());
                orderForList = Castings.OrderForList_ConvertIEnumerableToObservable(bl!.Order.GetOrders());
            } 
            CatalogForManager.DataContext = productForList;
            ListOfOrdersForManager.DataContext = orderForList;
        }

        #region Operations on Inventory - Manager Access
        private void CatalogForManager_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductWindowForOperations productWindowForOperations = new(productForList, (sender as DataGrid)?.SelectedItem as PO.ProductForList);
            productWindowForOperations.Show();
            productWindowForOperations.AddProduct.Visibility = Visibility.Collapsed;
            productWindowForOperations.UpdateProduct.Visibility = Visibility.Visible;
            productWindowForOperations.ProductID.IsEnabled = false;
            GroupArtistsBtn.IsEnabled = false;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductWindowForOperations productWindowForOperations = new(productForList);
            productWindowForOperations.Show();
            //    //open operations window in add mode
            //    ProductWindowForOperations productWindowForOperations = new();
            //    productWindowForOperations.Show();
            //        //this.Close();//close catalog window
            //  //ProductListView.ItemsSource = bl1!.Product.GetProductList();//update product view 
            //button visibility
            productWindowForOperations.UpdateProduct.Visibility = Visibility.Collapsed;
            productWindowForOperations.AddProduct.Visibility = Visibility.Visible;
            GroupArtistsBtn.IsEnabled = true;
        }

        private void StyleSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem? item = StyleSelector.SelectedItem as ComboBoxItem;
            switch ((string?)item?.Content)//filter by category
            {
                case "Realism"://all products of the "Realism" category
                    CatalogForManager.ItemsSource = productForList?.Where(x=>x?.Style == PO.ArtStyles.Realism);
                    break;
                case "Cartoon"://all products of the "Cartoon" category
                    CatalogForManager.ItemsSource = productForList?.Where(x => x?.Style == PO.ArtStyles.Cartoon);
                    break;
                case "Semi-Realism"://all products of the "SemiRealism" category
                    CatalogForManager.ItemsSource = productForList?.Where(x => x?.Style == PO.ArtStyles.SemiRealism);
                    break;
                case "Cubism"://all products of the "Cubism" category
                    CatalogForManager.ItemsSource = productForList?.Where(x => x?.Style == PO.ArtStyles.Cubism);
                    break;
                case "Abstract"://all products of the "Abstract" category
                    CatalogForManager.ItemsSource = productForList?.Where(x => x?.Style == PO.ArtStyles.Abstract);
                    break;
                case "None"://No filter
                    CatalogForManager.ItemsSource = productForList;
                    break;
            }
            GroupArtistsBtn.IsEnabled = false;
        }
        #region Artist Grouping
        private void GroupArtistsBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveGroupingsBtn_Click(sender, e);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CatalogForManager.ItemsSource);
            PropertyGroupDescription groupDescription = new("Artist");
            SortDescription sortDscription = new("Artist", ListSortDirection.Ascending);
            view.GroupDescriptions.Add(groupDescription);
            view.SortDescriptions.Add(sortDscription);
            GroupArtistsBtn.IsEnabled = false;
        }
        private void RemoveGroupingsBtn_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CatalogForManager.ItemsSource);
            view.GroupDescriptions.Clear();
            GroupArtistsBtn.IsEnabled = true;
        }
        private void ArtistNameFilterCbBx_DropDownOpened(object sender, EventArgs e)
        {
            ArtistNameFilterCbBx.Items.Clear();
            IEnumerable<IGrouping<string?, BO.ProductForList?>?>? groupings = bl1?.Product.GetAll_GroupedByArtistName_Manager();
            groupings = groupings?.OrderBy(p => p?.Key);
            foreach (var group in groupings!)
                ArtistNameFilterCbBx.Items.Add(group?.Key);
        }

        private void ArtistNameFilterCbBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CatalogForManager.DataContext = productForList?.Where(x => x?.Artist == ((ArtistNameFilterCbBx.SelectedItem) as string));
        }
        #endregion

        #region ascending & descending button click events
        private void PriceDESCBtn_Click(object sender, RoutedEventArgs e)
        {
            CatalogForManager.DataContext = OrderByDESC();
        }

        private void PriceACCBtn_Click(object sender, RoutedEventArgs e)
        {
            CatalogForManager.DataContext = OrderByASC();
        }
        #endregion  
        
        #region accending & descending price lamda expressions
        private IEnumerable<PO.ProductForList?>? OrderByASC() => productForList?.OrderBy(x => x?.Price);
        
        private IEnumerable<PO.ProductForList?>? OrderByDESC() => productForList?.OrderByDescending(x => x?.Price);
        #endregion
        #endregion

        #region operations on Order list - Manager Access
        private void ListOfOrdersForManager_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //update selected order 
            OrderOperationsWindow orderOperationsWindow = new((sender as ListView)?.SelectedItem as PO.OrderForList);
            orderOperationsWindow.Show();
            GroupArtistsBtn.IsEnabled = true;
        }


        #endregion


    }
}
