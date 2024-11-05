using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Xaml;

namespace PL.Product
{
    //public class FalseTotrueVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        Visibility visibility = (Visibility)value;
    //        if (visibility == Visibility.Collapsed)
    //        {
    //            return Visibility.Visible;
    //        }
    //        else
    //            return Visibility.Collapsed;
    //        //throw new NotImplementedException();
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    /// <summary>
    /// Interaction logic for ProductWindowForOperations.xaml
    /// </summary>
    public partial class ProductWindowForOperations : Window
    {
        public BlApi.IBl? bl1 = BlApi.Factory.GetBl();
        ObservableCollection<PO.ProductForList?>? forLists;
        BO.Product? productToAdd;
        PO.Product? productForOperations;
        private enum ArtStyles { None, Realism, Cartoon, SemiRealism, Cubism, Abstract }

        public ProductWindowForOperations(ObservableCollection<PO.ProductForList?>? listOfProd, PO.ProductForList? prod = null)
        {
            InitializeComponent();
            forLists = listOfProd;
            if (prod is not null)
            {
                productForOperations = Display_Update(prod);
                DataContext = productForOperations;
            }

        }
        #region Text input checks
        private void ProductID_TextChanged(object sender, TextChangedEventArgs e)//check correct input of ProductID
        {
            //make sure doesnt exist in dal already
            foreach (char i in ProductID.Text)
            {
                if (!(i == '0' || i == '1' || i == '2' || i == '3' || i == '4' || i == '5' || i == '6' || i == '7' || i == '8' || i == '9'))
                {
                    MessageBox.Show("incorrect id format");
                    this.Close();
                    return;
                }
            }
            if (ProductID.Text.Length == 0)
            {
                MessageBox.Show("must enter the id number of a product");
                return;
            }
        }

        private void ProductName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProductName.Text.Length == 0)
            {
                ProductName.Text = "Unknown";
                MessageBox.Show("must enter a name for the product");
                this.Close();
                return;
            }
        }
        private void ArtistName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ArtistName.Text.Length == 0)
            {
                ArtistName.Text = "Unknown";
                this.Close();
                return;
            }
        }

        private void ProductPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (char i in ProductPrice.Text)
            {
                if (!(i == '0' || i == '1' || i == '2' || i == '3' || i == '4' || i == '5' || i == '6' || i == '7' || i == '8' || i == '9' || i == '.'))
                {
                    MessageBox.Show("incorrect id format");
                    this.Close();
                    return;
                }
            }
            string helper = ProductPrice.Text;
            int countOfPoint = 0;
            foreach (char i in helper)
            {
                if (i == '.')
                    countOfPoint++;
            }
            if (countOfPoint > 1)
            {
                MessageBox.Show("incorrect format for price");
                this.Close(); return;
            }
        }

        private void ProductCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //nothing to check
        }

        private void ProductAmountInStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (char i in ProductAmountInStock.Text)
            {
                if (!(i == '0' || i == '1' || i == '2' || i == '3' || i == '4' || i == '5' || i == '6' || i == '7' || i == '8' || i == '9'))
                {
                    MessageBox.Show("incorrect format for amount in stock");
                    this.Close(); return;
                }
            }
        }
        #endregion
        #region update display for product - helper method
        private PO.Product? Display_Update(PO.ProductForList? prod)
        {
            if (prod != null) 
            {
                BO.Product? products = bl1?.Product.GetProductByIdForManager(prod.ID);
                productForOperations = new()
                {
                    ID = products!.ID,
                    Title = products!.NameOfProduct,
                    Amount = products!.AmountInStock,
                    Style = (PO.ArtStyles?)products!.Categories,
                    Artist = products!.Artist,
                    Price = products!.Price,
                    Image = products!.Image
                };
                return productForOperations;
                //-----------setting pre entered information--------------
                ////ID button
                //ProductID.Text = $"{prod.ID}";
                //ProductID.IsEnabled = false;
                ////Names of product and artist
                //ProductName.Text = prod.Title;
                //ProductName.IsEnabled = false;

                //ArtistName.Text = prod.Artist;
                //ArtistName.IsEnabled = false;
                ////-----------changeable elements-------------
                //ProductPrice.Text = $"{prod.Price}";
                //ProductCategory.Text = $"{prod.Style}";
                //ProductAmountInStock.Text = $"{products!.AmountInStock}";
                
            }
            return null;
        }
        #endregion
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try//add product to DAL
            {
                productToAdd = new BO.Product
                {
                    ID = int.Parse(ProductID.Text),
                    NameOfProduct = ProductName.Text,
                    Artist = ArtistName.Text,
                    Price = double.Parse(ProductPrice.Text),
                    AmountInStock = int.Parse(ProductAmountInStock.Text)
                };
                switch (ProductCategory.Text)
                {
                    case "None"://no category selected
                        productToAdd.Categories = BO.Enums.ArtStyles.None;
                        break;
                    case "Realism"://all products of the "Realism" category
                        productToAdd.Categories = BO.Enums.ArtStyles.Realism;
                        break;
                    case "Cartoon"://all products of the "Cartoon" category
                        productToAdd.Categories = BO.Enums.ArtStyles.Cartoon;
                        break;
                    case "Semi-Realism"://all products of the "SemiRealism" category
                        productToAdd.Categories = BO.Enums.ArtStyles.SemiRealism;
                        break;
                    case "Cubism"://all products of the "Cubism" category
                        productToAdd.Categories = BO.Enums.ArtStyles.Cubism;
                        break;
                    case "Abstract"://all products of the "Abstract" category
                        productToAdd.Categories = BO.Enums.ArtStyles.Abstract;
                        break;
                }
                bl1!.Product.Add(productToAdd);
                // MessageBox.Show("Product was added successfully");
                this.Close();
                //show updated window

                ProductForList winProducts = new(bl1!);
                winProducts.Show();
            }
            catch (BO.FormatIsIncorrectException ex)//incorrect format thrown by add function
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            catch (BO.DoubleFoundException ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void UpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            try //update product in DAL
            {
                BO.Product prod_to_update = new()
                {
                    ID = Convert.ToInt32(ProductID.Text),
                    NameOfProduct = ProductName.Text,
                    Artist = ArtistName.Text,
                    Price = Convert.ToDouble(ProductPrice.Text),
                    AmountInStock = Convert.ToInt32(ProductAmountInStock.Text)
                };
                switch (ProductCategory.Text)
                {
                    case "None"://no category selected
                        prod_to_update.Categories = BO.Enums.ArtStyles.None;
                        break;
                    case "Realism"://all products of the "Realism" category
                        prod_to_update.Categories = BO.Enums.ArtStyles.Realism;
                        break;
                    case "Cartoon"://all products of the "Cartoon" category
                        prod_to_update.Categories = BO.Enums.ArtStyles.Cartoon;
                        break;
                    case "Semi-Realism"://all products of the "SemiRealism" category
                        prod_to_update.Categories = BO.Enums.ArtStyles.SemiRealism;
                        break;
                    case "Cubism"://all products of the "Cubism" category
                        prod_to_update.Categories = BO.Enums.ArtStyles.Cubism;
                        break;
                    case "Abstract"://all products of the "Abstract" category
                        prod_to_update.Categories = BO.Enums.ArtStyles.Abstract;
                        break;
                }
                bl1!.Product.Update(prod_to_update);

                //update works
                //MessageBox.Show("Update was successful");
                this.Close();
                //show updated window
                ProductForList winProducts = new(bl1!);
                winProducts.Show();
            }
            //exceptions from BL
            catch (BO.ObjectNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            catch (BO.FormatIsIncorrectException ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
    }
}
