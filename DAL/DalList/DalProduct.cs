using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using DalApi;
using DO;
namespace Dal;

internal class DalProduct : IProduct
{
    //---------------Data changing functions----------------
    #region Data Changing Functions - Add, Delete, Update
    /// <summary>
    /// Adds a new "item" to the list of products
    /// </summary>
    public int Add(Product item)
    {
        int ind = DataSource.DSProducts.FindIndex(x => x?.ID == item.ID);
        
        //1 - adding new item
        if (ind == -1) 
        {
            DataSource.DSProducts.Add(item);
            return item.ID;
        }
        
        //2 - the item already exists but is deleted, meaning "add" is called from "update"
        if (DataSource.DSProducts[ind]?.IsDeleted == true)
        {   
            DataSource.DSProducts[ind] = item;
            return item.ID;
        }

        //3 - the ID num already in use, adding isn't allowed
        //throw new Exception("Unothorized Override");
        throw new DoubleFoundException("Product ID number already in use");
    }

    /// <summary>
    /// delete "item" from the list of products
    /// </summary>
    public void Delete(int id)
    {
        int ind = DataSource.DSProducts.FindIndex(x => x?.ID == id);
        
        //object never existed or aleardy been deleted
        if (ind == -1 || DataSource.DSProducts[ind]?.IsDeleted == true)
            throw new ObjectNotFoundException("Product does not exist, cannot be deleted");

        Product p = (Product)DataSource.DSProducts[ind]!;//places in helper variable
        p.IsDeleted = true;
        DataSource.DSProducts[ind] = p; //updates "IsDeleted" to true in the products collection
    }

    /// <summary>
    /// update product with same id as "item" with "item info
    /// </summary>
    public void Update(Product item)
    {
        try
        {
            GetById(item.ID);
        } //check if exist by calling request method
        catch(ObjectNotFoundException)
        {
            throw new ("Product you wish to update does not exist");
        }
        //item exist, update by deleting and adding again
        Delete(item.ID);
        Add(item);
    }
    #endregion

    //---------------Get functions----------------
    #region Get Functions - GetAll, GetById, GetByFilter


    /// <summary>
    /// returns the entire collection of products
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter) =>
        (filter == null ?
        DataSource.DSProducts.Where(item => item?.IsDeleted == false) :
        DataSource.DSProducts.Where(item => item?.IsDeleted == false).Where(filter)) ??
        throw new ObjectNotFoundException("Products do not exist");

    /// <summary>
    /// returns product with the same "id" as the one given
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public Product GetById(int id) =>
        DataSource.DSProducts.Find(x => x?.IsDeleted == false && x?.ID == id) ??
        throw new ObjectNotFoundException("Product does not exist");

    /// <summary>
    /// returns first product that match the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public Product GetByFilter(Func<Product?, bool> filter) =>
        DataSource.DSProducts.First(filter) ?? throw new ObjectNotFoundException("Product does not exist");
    #endregion
}
