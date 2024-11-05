using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using DalApi;

namespace BlImplementation;

internal class BlProduct : BlApi.IProduct
{
    readonly private static DalApi.IDal? dal = DalApi.Factory.Get();

    //---------------Converter functions----------------
    #region Converter Functions - DO.Product=>BO.ProductForList, =>BO.Product, =>BO.ProductItem. BO.Product=>DO.Product
    /// <summary>
    /// convert to ProductForList
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ObjectDoesNotExistException"></exception>
    public static BO.ProductForList ConvertToP(DO.Product? t)
    {
        if (t!= null)
        {
            BO.ProductForList helper = new()
            {
                ID = t?.ID ?? throw new BO.NullPropertyException("Null product ID"),
                NameOfProduct = t?.Name,
                Artist = t?.Artist,
                Price = t?.Price ?? throw new BO.NullPropertyException("Null product price"),
                Categories = (BO.Enums.ArtStyles)t?.Categories!,
                Image = t?.Image
            };
            return helper;
        }
        throw new BO.ObjectNotFoundException("The product you wish to convert does not exist");//t is not an existing product
    }
    /// <summary>
    /// convert to BO.product
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ObjectDoesNotExistException"></exception>
    public static BO.Product ConvertToProduct(DO.Product? t)
    {
        if (t != null)
        {
            BO.Product helper = new()
            {
                ID = t?.ID ?? throw new BO.NullPropertyException("Null product ID"),
                NameOfProduct = t?.Name,
                Artist= t?.Artist,
                Categories = (BO.Enums.ArtStyles)t?.Categories!,
                AmountInStock = t?.InStock ?? 0,
                Price = t?.Price ?? throw new BO.NullPropertyException("Null product price"),
                Image = t?.Image
            };
            return helper;
        }
        throw new BO.ObjectNotFoundException("The product you wish to convert does not exist");//t is not an existing product
    }    
    /// <summary>
    /// convert to ProductItem
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ObjectDoesNotExistException"></exception>
    public static BO.ProductItem ConvertToPItem(DO.Product? t)
    {
        if (t != null)
        {
            BO.ProductItem helper = new()
            {
                ID = t?.ID ?? throw new BO.NullPropertyException("Null product ID"),
                NameOfProduct = t?.Name,
                Artist= t?.Artist,
                Categories = (BO.Enums.ArtStyles)t?.Categories!,
                Price = t?.Price ?? throw new BO.NullPropertyException("Null product price"),
                InStock = (t?.InStock > 0),
                Image = t?.Image
            };
            return helper;
        }
        throw new BO.ObjectNotFoundException("The product you wish to convert does not exist");//t is not an existing product
    }
    /// <summary>
    /// convert from BO to DO (Product)
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ObjectDoesNotExistException"></exception>
    public static DO.Product ConvertToDOP(BO.Product? t)
    {
        if (t != null)
        {
            DO.Product helper = new()
            {
                ID = t?.ID ?? throw new BO.NullPropertyException("Missing product ID"),
                Name = t?.NameOfProduct ?? throw new BO.NullPropertyException("Missing product name"),
                Artist = t?.Artist ?? throw new BO.NullPropertyException("Missing product artist name"),
                Categories = (t.Categories != BO.Enums.ArtStyles.None) ?
                    (DO.Enums.ArtStyles)t.Categories :
                    throw new BO.NullPropertyException("Missing product category"),
                Price = (t?.Price > 0) ? t.Price : throw new BO.NullPropertyException("Missing product price"),
                InStock = (t?.AmountInStock >= 0) ? t.AmountInStock : throw new BO.NullPropertyException("Missing product amount"),
                Image = t?.Image,
                IsDeleted = false
            };
            return helper;
        }
        throw new BO.ObjectNotFoundException("The product you wish to convert does not exist");//t is not an existing product
    }
    #endregion
    //---------------Get functions----------------
    #region Get Functions - ProductList, ForManager, ForCustomer(&Catalog)
    public IEnumerable<BO.ProductForList?> GetProductList()
    {
        try
        {
            IEnumerable<DO.Product?>? products = dal?.Product.GetAll();//get all products
            return from DO.Product prod in products! select ConvertToP(prod);//build the list of ProductForList
        }
        catch (DalApi.ObjectNotFoundException ex)//could not get the object
        {
            throw new BO.ObjectNotFoundException("Product does not exist", ex);
        }
        catch (BO.ObjectNotFoundException)//product doesn't exist -> couldn't convert product from DO to BO
        {
            throw;
        }
        catch (BO.NullPropertyException ex)//product doesn't exist -> couldn't convert product from DO to BO
        {
            throw new BO.ObjectNotFoundException("Product does not exist", ex);
        }
    }
    public IEnumerable<BO.ProductItem?> GetProductItemList()
    {
        try
        {
            IEnumerable<DO.Product?>? products = dal?.Product.GetAll();//get all products
            return from DO.Product prod in products! select ConvertToPItem(prod);//build the list of ProductForList
        }
        catch (DalApi.ObjectNotFoundException ex)//could not get the object
        {
            throw new BO.ObjectNotFoundException("Product does not exist", ex);
        }
        catch (BO.ObjectNotFoundException)//product doesn't exist -> couldn't convert product from DO to BO
        {
            throw;
        }
        catch (BO.NullPropertyException ex)//product doesn't exist -> couldn't convert product from DO to BO
        {
            throw new BO.ObjectNotFoundException("Product does not exist", ex);
        }
    }
    
    public BO.Product? GetProductByIdForManager(int id)
    {
        if (id > 0)//id is logical (could exist)
        {
            try
            {
                DO.Product? p = dal?.Product.GetById(id);//get the product from DAL
                return ConvertToProduct(p);//return the conversion
            }
            catch (DalApi.ObjectNotFoundException ex)//get fuction didn't work - doesn't exist in DAL
            {
                throw new BO.ObjectNotFoundException("Product does not exist", ex);
            }
            catch (BO.NullPropertyException ex)//get fuction didn't work - doesn't exist in DAL
            {
                throw new BO.ObjectNotFoundException("Product does not exist", ex);
            }
        }
        throw new BO.FormatIsIncorrectException("id cannot be smaller than (or) 0");
    }
    
    public BO.ProductItem? GetProductByIdForCustomer(int id, BO.Cart? c)
    {
        if (id > 0) 
        {
            try
            {
                DO.Product? dProduct = dal?.Product.GetById(id);//get the product from DAL
                BO.ProductItem result = ConvertToPItem(dProduct);//convert
                result.AmountInCart = c?.items?.Where(x => x?.ProductID == id).Sum(x => x?.Amount) ?? 0;//calculate amount in cart
                return result;
            }
            catch (DalApi.ObjectNotFoundException ex) 
            { 
                throw new BO.ObjectNotFoundException("Product does not exist", ex); 
            }
            catch (BO.ObjectNotFoundException ex) 
            { 
                throw new BO.ObjectNotFoundException("Product does not exist", ex); 
            }
            catch (BO.NullPropertyException ex) 
            { 
                throw new BO.ObjectNotFoundException("Product does not exist", ex); 
            }
        }
        throw new BO.FormatIsIncorrectException("id cannot be smaller than (or) 0");
    }
    
    public IEnumerable<IGrouping<BO.Enums.ArtStyles, BO.ProductForList?>> GetAll_GroupedByCategory_Manager()
    {
        try
        {
            IEnumerable<IGrouping<BO.Enums.ArtStyles, BO.ProductForList?>> products =
            GetProductList().GroupBy(
                x => x!.Categories,
             x => new BO.ProductForList()
             {
                 ID = x!.ID,
                 NameOfProduct = x.NameOfProduct,
                 Artist = x.Artist,
                 Price = x.Price,
                 Categories = x.Categories,
                 Image = x.Image
             });
            return products;
        }
        catch(BO.ObjectNotFoundException)
        {
            throw;
        }        
    }
    
    public IEnumerable<IGrouping<string?, BO.ProductForList?>> GetAll_GroupedByArtistName_Manager()
    {
        try
        {
            IEnumerable<IGrouping<string?, BO.ProductForList?>> products =
            GetProductList().GroupBy(
                x => x!.Artist,
                x => new BO.ProductForList()
                {
                    ID = x!.ID,
                    NameOfProduct = x.NameOfProduct,
                    Artist = x.Artist,
                    Price = x.Price,
                    Categories = x.Categories,
                    Image = x.Image
                });
            return products;
        }
        catch (BO.ObjectNotFoundException)
        {
            throw;
        }
    }

    public IEnumerable<IGrouping<BO.Enums.ArtStyles, BO.ProductItem?>> GetAll_GroupedByCategory_Customer()
    {
        try
        {
            IEnumerable<IGrouping<BO.Enums.ArtStyles, BO.ProductItem?>> products = dal?.Product.GetAll().GroupBy(
                x => (BO.Enums.ArtStyles)(x?.Categories)!,
                x => new BO.ProductItem()
                {
                    ID = ((DO.Product)x!).ID,
                    NameOfProduct = ((DO.Product)x!).Name,
                    Artist = ((DO.Product)x!).Artist,
                    Price = ((DO.Product)x!).Price,
                    Categories = (BO.Enums.ArtStyles)((DO.Product)x!).Categories,
                    InStock = ((DO.Product)x!).InStock > 0,
                    AmountInCart = 0,
                    Image = ((DO.Product)x!).Image
                })!;
            return products;
        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Product does not exist", ex);
        }
    }

    public IEnumerable<IGrouping<string?, BO.ProductItem?>> GetAll_GroupedByArtistName_Customer()
    {
        try
        {
            IEnumerable<IGrouping<string?, BO.ProductItem?>> products = dal?.Product.GetAll().GroupBy(
                x => ((DO.Product)x!).Artist,
                x => new BO.ProductItem()
                {
                    ID = ((DO.Product)x!).ID,
                    NameOfProduct = ((DO.Product)x!).Name,
                    Artist = ((DO.Product)x!).Artist,
                    Price = ((DO.Product)x!).Price,
                    Categories = (BO.Enums.ArtStyles)((DO.Product)x!).Categories,
                    InStock = ((DO.Product)x!).InStock > 0,
                    AmountInCart = 0,
                    Image = ((DO.Product)x!).Image
                })!;
            return products;
        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Product does not exist", ex);
        }
    }
    
    #endregion

    //---------------Data changing functions----------------
    #region Data Changing Functions - Add, Delete, Update
    public void Add(BO.Product prod)
    {
        //if the product exists in a correct format try and add the product to the DAL
        if (prod.ID > 0 && prod.NameOfProduct != "" && prod.Artist != "" 
            && prod.Categories != BO.Enums.ArtStyles.None && prod.Price > 0 && prod.AmountInStock >= 0)
        {
            try
            {
                dal?.Product.Add(ConvertToDOP(prod));
            }
            catch (DalApi.DoubleFoundException ex)//if the product already exists in the DAL
            {
                throw new BO.DoubleFoundException("Cannot add an already existing product", ex);
            }
            catch (BO.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException("Product does not exist", ex);
            }
            catch (BO.NullPropertyException ex)
            {
                throw new BO.ObjectNotFoundException("Product does not exist", ex);
            }
        }
        else
            throw new BO.FormatIsIncorrectException("Cannot add an incorrectly formated object");
    }
    
    public void Delete(int idProd)
    {
        IEnumerable<DO.OrderItem?>? orderChecks = dal?.OrderItem.GetAll();//get all orders that have not been deleted
        if (orderChecks?.Any(x => x?.ProductID == idProd) == false)//the product does not exist in the list of orders
        {
            try
            {
                dal?.Product.Delete(idProd); 
            }
            catch(DalApi.ObjectNotFoundException ex) 
            { 
                throw new BO.ObjectNotFoundException("This product does not exist", ex); 
            }
        }
        else //the item exists in the Orders
            throw new BO.CouldNotDeleteObjectException("Cannot delete a product if it exists in an existing order");
    }

    public void Update(BO.Product prod)
    {
        if (prod.ID > 0 && prod.NameOfProduct != "" && prod.Artist != "" && prod.Categories != BO.Enums.ArtStyles.None
            && prod.Price > 0 && prod.AmountInStock >= 0)
        {
            try
            {
                dal?.Product.Update(ConvertToDOP(prod));
            }
            catch(DalApi.ObjectNotFoundException ex) 
            { 
                throw new BO.ObjectNotFoundException("No such product exist", ex); 
            }
            catch (BO.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException("Product does not exist", ex);
            }
            catch (BO.NullPropertyException ex)
            {
                throw new BO.ObjectNotFoundException("Product does not exist", ex);
            }
        }
        else
            throw new BO.FormatIsIncorrectException("Cannot update an incorrectly formated object");
    }
    #endregion
}