using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
internal class OrderItem : IOrderItem
{
    private readonly string orderItemPath = @"OrderItem.xml";
    private readonly string configPath = @"Config.xml";

    #region OrderItem XmlSerializer
    #region Data changing methods
    public int Add(DO.OrderItem item)
    {
        List<DO.OrderItem?>? orderItemsList = XmlTool.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath);
        List<ImportentNumbers>? runningList = XmlTool.LoadListFromXMLSerializer<ImportentNumbers>(configPath);

        ImportentNumbers runningNum = (from number in runningList
                                       where (number.typeOfnumber == "Order Item running number")
                                       select number).FirstOrDefault();

        runningList?.Remove(runningNum);

        runningNum.numberSaved++;
        item.ID = (int)runningNum.numberSaved;

        runningList?.Add(runningNum);
        orderItemsList?.Add(item);

        XmlTool.SaveListToXMLSerializer(runningList!, configPath);
        XmlTool.SaveListToXMLSerializer(orderItemsList!, orderItemPath);

        return (int)runningNum.numberSaved;
    }

    public void Delete(int id)
    {
        var listOrderItems = XmlTool.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath);
        if (listOrderItems?.Where(p => p.ID == id).FirstOrDefault().ID == 0)
            throw new ObjectNotFoundException("missing id");
        listOrderItems?.Single(x => x.ID == id);
        int? indexToOrder = listOrderItems?.FindIndex(x => x.ID == id);
        if (indexToOrder == null)
            throw new ObjectNotFoundException("missing id");
        List<DO.OrderItem?>? updatedListOrderItems = new();
        foreach (var item in listOrderItems!)
        {
            if (item.ID == id)
            {
                updatedListOrderItems.Add(new DO.OrderItem
                {
                    ID = id,
                    ProductID = item.ProductID,
                    Price = item.Price,
                    Amount = item.Amount,
                    OrderID = item.OrderID,
                    Image = item.Image,
                    IsDeleted = item.IsDeleted
                });
            }
            updatedListOrderItems.Add(item);
        }
        listOrderItems.RemoveAll(x => x.ID != 0);
        XmlTool.SaveListToXMLSerializer(updatedListOrderItems, orderItemPath);
    }
    public void Update(DO.OrderItem item)
    {
        var listOrders = XmlTool.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath);
        if (listOrders is null)
            throw new LoadingException("orders could not be loaded");

        DO.OrderItem o = listOrders!.FirstOrDefault(p => p.ID == item.ID);

        listOrders?.Remove(o);
        listOrders?.Add(item);
        XmlTool.SaveListToXMLSerializer(listOrders!, orderItemPath);
    }
    #endregion
    #region Get methods
    public IEnumerable<DO.OrderItem?> GetAll(Func<DO.OrderItem?, bool>? filter = null)
    {
        List<DO.OrderItem?> listOrders = XmlTool.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath)!;
        return filter == null ? listOrders.OrderBy(ordIt => ((DO.OrderItem)ordIt!).ID)
                              : listOrders.Where(filter).OrderBy(ordIt => ((DO.OrderItem)ordIt!).ID);
    }

    public IEnumerable<DO.OrderItem?> GetAllItemsInOrder(int orderNumber)
    {
        List<DO.OrderItem?> listOrders = XmlTool.LoadListFromXMLSerializer<DO.OrderItem?>(orderItemPath)!;
        return listOrders.Where(x => ((DO.OrderItem)x!).OrderID == orderNumber).OrderBy(x => ((DO.OrderItem)x!).ID);
        //return filter == null ? listOrders.OrderBy(ordIt => ((DO.OrderItem)ordIt!).ID)
        //                      : listOrders.Where(filter).OrderBy(ordIt => ((DO.OrderItem)ordIt!).ID);
    }

    public DO.OrderItem GetByFilter(Func<DO.OrderItem?, bool> filter)
    {
        IEnumerable<DO.OrderItem?> helperIEnumerable = GetAll(filter);
        return (DO.OrderItem)helperIEnumerable.FirstOrDefault()!;
    }

    public DO.OrderItem GetById(int id) =>
        XmlTool.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath)!.FirstOrDefault(p => p.ID == id);
    //{
    //    throw new NotImplementedException();
    //}

    public DO.OrderItem GetItem(int orderNumber, int productNumber) =>
        XmlTool.LoadListFromXMLSerializer<DO.OrderItem>(orderItemPath)!.FirstOrDefault(p => p.ID == orderNumber && p.ProductID == productNumber);
    
    #endregion
    #endregion
}
