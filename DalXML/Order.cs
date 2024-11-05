using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

internal class Order : IOrder
{
    private readonly string orderPath = @"Order.xml";
    private readonly string configPath = @"Config.xml";

    #region Order XmlSerializer
    #region Data changing methods
    public int Add(DO.Order item) //finished
    {
        //var listOrders = XmlTool.LoadListFromXMLSerializer<DO.Order>(orderPath);

        //if (listOrders!.Exists(ord => ord.ID == item.ID))
        //    throw new Exception("id already exist");//DalAlreadyExistIdException(lecturer.ID, "Lecturer");

        //listOrders.Add(item);

        //XmlTool.SaveListToXMLSerializer(listOrders, orderPath);

        //return item.ID;

        List<DO.Order?>? ordersList = XmlTool.LoadListFromXMLSerializer<DO.Order?>(orderPath);
        List<ImportentNumbers>? runningList = XmlTool.LoadListFromXMLSerializer<ImportentNumbers>(configPath);

        ImportentNumbers runningNum = (from number in runningList
                                       where (number.typeOfnumber == "Order running number")
                                       select number).FirstOrDefault();

        runningList?.Remove(runningNum);

        runningNum.numberSaved++;
        item.ID = (int)runningNum.numberSaved;

        runningList?.Add(runningNum);
        ordersList?.Add(item);

        XmlTool.SaveListToXMLSerializer(runningList!, configPath);
        XmlTool.SaveListToXMLSerializer(ordersList!, orderPath);

        return (int)runningNum.numberSaved;
    }

    public void Delete(int id) // needs work
    {
        var listOrders = XmlTool.LoadListFromXMLSerializer<DO.Order>(orderPath);
        if (listOrders?.Where(p => p.ID == id).FirstOrDefault().ID == 0)
            throw new ObjectNotFoundException("missing id"); 
        listOrders?.Single(x=> x.ID == id);
        int? indexToOrder = listOrders?.FindIndex(x => x.ID == id);
        if (indexToOrder == null)
            throw new ObjectNotFoundException("missing id");
        List<DO.Order?>? updatedListOrders = new();
        foreach (var item in listOrders!)
        {
            if (item.ID == id)
            {
                updatedListOrders.Add(new DO.Order
                { 
                    IsDeleted = true, 
                    ID = id, 
                    CustomerName = item.CustomerName, 
                    CustomerAddress = item.CustomerAddress, 
                    CustomerEmail = item.CustomerEmail, 
                    DeliveryDate = item.DeliveryDate, 
                    OrderDate = item.OrderDate, 
                    ShipDate = item.ShipDate 
                });
            }
            updatedListOrders.Add(item);
        }
        listOrders.RemoveAll(x => x.ID != 0);
        XmlTool.SaveListToXMLSerializer(updatedListOrders, orderPath);
    }
    public void Update(DO.Order item)
    {
        //Delete(item.ID);
        //Add(item);
        var listOrders = XmlTool.LoadListFromXMLSerializer<DO.Order>(orderPath);
        if(listOrders is null)
            throw new LoadingException("orders could not be loaded");

        DO.Order o = listOrders!.FirstOrDefault(p => p.ID == item.ID);
        
        listOrders?.Remove(o);
        listOrders?.Add(item);
        XmlTool.SaveListToXMLSerializer(listOrders!, orderPath);
    }
    #endregion
    #region Get methods
    public IEnumerable<DO.Order?> GetAll(Func<DO.Order?, bool>? filter = null)
    {
        List<DO.Order?> listOrders = XmlTool.LoadListFromXMLSerializer<DO.Order?>(orderPath)!;
        return filter == null ? listOrders.OrderBy(ord => ((DO.Order)ord!).ID)
                              : listOrders.Where(filter).OrderBy(ord => ((DO.Order)ord!).ID);
    }

    public DO.Order GetByFilter(Func<DO.Order?, bool> filter)
    {
        IEnumerable<DO.Order?> helperIEnumerable = GetAll(filter);
        return (DO.Order)helperIEnumerable.FirstOrDefault()!;
    }

    public DO.Order GetById(int id) =>
        XmlTool.LoadListFromXMLSerializer<DO.Order>(orderPath)!.FirstOrDefault(p => p.ID == id);
    #endregion
    #endregion
}
