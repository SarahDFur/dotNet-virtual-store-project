using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using DalApi;
using DO;
namespace Dal;

internal class DalOrderItem : IOrderItem
{
    //---------------Data changing functions----------------
    #region Data Changing Functions - Add, Delete, Update
    /// <summary>
    /// Adds a new "item" to the list of order items
    /// </summary>
    public int Add(OrderItem item)
    {
        //1 - addind new item
        if(item.ID == 0) 
        {
            item.ID = DataSource.Config.nextOrderItemNumber;
            DataSource.DSOrderItems.Add(item);
            return item.ID;
        }
        
        //2 - the item already exists but is deleted, meaning "add" is called from "update"
        int ind = DataSource.DSOrderItems.FindIndex(x => x?.ID == item.ID);
        if (DataSource.DSOrderItems[ind]?.IsDeleted == true)
        {
            DataSource.DSOrderItems[ind] = item;
            return item.ID;
        }
        
        //3 - the ID num already in use, adding isn't allowed
        throw new DoubleFoundException("Order item ID number already in use");
    }

    /// <summary>
    /// delete "item" from the list of order items
    /// </summary>
    public void Delete(int id)
    {
        int ind = DataSource.DSOrderItems.FindIndex(x => x?.ID == id);

        if (ind == -1 || DataSource.DSOrderItems[ind]?.IsDeleted == true)
            throw new ObjectNotFoundException("Order item does not exist, cannot be deleted");

        OrderItem o = (OrderItem)DataSource.DSOrderItems[ind]!; //places in helper variable
        o.IsDeleted = true;
        DataSource.DSOrderItems[ind] = o; //updates "IsDeleted" to true in the order items collection
    }

    /// <summary>
    /// update order item with same id as "item" with "item info
    /// </summary>
    public void Update(OrderItem item)
    {
        try { GetById(item.ID); } //check if exist by calling request method
        catch(ObjectNotFoundException)
        {
            throw new ObjectNotFoundException("Order item you wish to update does not exist");
        }
        //item exist, update by deleting and adding again
        Delete(item.ID);
        Add(item);
    }
    #endregion

    //---------------Get functions----------------
    #region Get Functions - GetAll, GetById, GetByFilter, GetItem, getAllItemsInOrder

    /// <summary>
    /// returns the entire collection of order items
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter) =>
        (filter == null ?
        DataSource.DSOrderItems.Where(item => item?.IsDeleted == false).ToList() :
        DataSource.DSOrderItems.Where(item => item?.IsDeleted == false).Where(filter).ToList()) ??
        throw new ObjectNotFoundException("Order items do not exist");


    /// <summary>
    /// returns order item with the same "id" as the one given
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public OrderItem GetById(int id) =>
        DataSource.DSOrderItems.Find(x => x?.IsDeleted == false && x?.ID == id) ??
        throw new ObjectNotFoundException("Order item does not exist");

    /// <summary>
    /// returns first orderItem that match the filter
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public OrderItem GetByFilter(Func<OrderItem?, bool> filter) =>
        DataSource.DSOrderItems.First(filter) ?? throw new ObjectNotFoundException("Order item does not exist");


    /// <summary>
    /// returns order item with the same orderID and productID as the one given
    /// </summary>
    /// <param name="orderNumber"></param>
    /// <param name="productNumber"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public OrderItem GetItem(int orderNumber, int productNumber) =>
        GetAllItemsInOrder(orderNumber).First(x => x?.ProductID == productNumber)
        ?? throw new ObjectNotFoundException("Order item does not exist");

    /// <summary>
    /// return all the items that have the same orderNumber as the one given
    /// </summary>
    /// <param name="orderNumber"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public IEnumerable<OrderItem?> GetAllItemsInOrder(int orderNumber) =>
        DataSource.DSOrderItems.Where
        (item => item?.IsDeleted == false).Where(item => item?.OrderID == orderNumber).Select(item => item).ToList()
        ?? throw new ObjectNotFoundException("Order items do not exist in this order");
    #endregion
}
