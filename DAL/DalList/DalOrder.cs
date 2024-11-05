using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DalApi;
using DO;
namespace Dal;

internal class DalOrder : IOrder
{
    //---------------Data changing functions----------------
    #region Data Changing Functions - Add, Delete, Update

    /// <summary>
    /// Adds a new "item" to the list of orders
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DoubleFoundException"></exception>
    public int Add(Order item)
    {
        //1 - addind new item
        if (item.ID == 0)
        {
            item.ID = DataSource.Config.nextOrderNumber;
            DataSource.DSOrders.Add(item);
            return item.ID;
        }

        //2 - the item already exists but is deleted, meaning "add" is called from "update"
        int ind = DataSource.DSOrders.FindIndex(x => x?.ID == item.ID);
        if (DataSource.DSOrders[ind]?.IsDeleted == true)
        {
            DataSource.DSOrders[ind] = item;
            return item.ID;
        }

        //3 - the ID num already in use, adding isn't allowed
        throw new DoubleFoundException("Order ID number already in use");
    }


    /// <summary>
    /// delete "item" from the list of orders
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="ObjectNotFoundException"></exception>
    public void Delete(int id)
    {
        int ind = DataSource.DSOrders.FindIndex(x => x?.ID == id);

        if (ind == -1 || DataSource.DSOrders[ind]?.IsDeleted == true)
            throw new ObjectNotFoundException("Order does not exist, cannot be deleted");

        Order o = (Order)DataSource.DSOrders[ind]!;//places in helper variable
        o.IsDeleted = true;
        DataSource.DSOrders[ind] = o; //updates "IsDeleted" to true in the order collection
    }

    /// <summary>
    /// update order with same id as "item" with "item info
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="ObjectNotFoundException"></exception>
    public void Update(Order item)
    {
        try { GetById(item.ID); } //check if exist by calling request method
        catch(ObjectNotFoundException)
        {
            throw new ObjectNotFoundException("Order you wish to update does not exist");
        }
        //item exist, update by deleting and adding again
        Delete(item.ID);
        Add(item);
    }
    #endregion

    //---------------Get functions----------------
    #region Get Functions - GetAll, GetById, GetByFilter

    /// <summary>
    /// returns the entire collection of orders
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
        (filter == null ?
        DataSource.DSOrders.Where(item => item?.IsDeleted == false).ToList() :
        DataSource.DSOrders.Where(item => item?.IsDeleted == false).Where(filter).ToList()) ??
        throw new ObjectNotFoundException("Orders do not exist");

    /// <summary>
    /// returns order with the same "id" as the one given
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public Order GetById(int id) =>
        DataSource.DSOrders.Find(x => x?.IsDeleted == false && x?.ID == id) ??
        throw new ObjectNotFoundException("Order does not exist");

    /// <summary>
    /// returns first order that match the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public Order GetByFilter(Func<Order?, bool> filter) =>
        DataSource.DSOrders.First(filter) ?? throw new ObjectNotFoundException("Order does not exist");

    #endregion
}
