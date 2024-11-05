using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

public struct ImportentNumbers
{
    public double numberSaved { get; set; }
    public string typeOfnumber { get; set; }
}
sealed internal class DalXml : IDal
{
    #region singleton
   // public static readonly IDal instance = new DalXml();
    public static IDal Instance { get; } = new DalXml();

    DalXml() 
    {
        //List<ImportentNumbers> configs = new()
        //{
        //    new ImportentNumbers() { numberSaved = DataSource.Config.s_startOrderItemNumber, typeOfnumber = "Order Item running number" },
        //    new ImportentNumbers() { numberSaved = DataSource.Config.s_startOrderItemNumber, typeOfnumber = "Order running number" }
        //};
        //XmlTool.SaveListToXMLSerializer<ImportentNumbers>(configs, configPath);
        ////List<DO.Order> orders = new();
        ////orders.Add(new DO.Order() { ID = 1001, IsDeleted = false, OrderDate = DateTime.Now });

        //foreach (var item in DataSource.DSProducts)
        //{
        //    CreateProduct((DO.Product)item!);
        //}
        ////XmlTool.SaveListToXMLSerializer<DO.Product?>(DataSource.DSProducts, productPath);
        //XmlTool.SaveListToXMLSerializer<DO.Order?>(DataSource.DSOrders, orderPath);
        //XmlTool.SaveListToXMLSerializer<DO.OrderItem?>(DataSource.DSOrderItems, orderItemPath);
    }
    static DalXml() { }
    //public void CreateProduct(DO.Product productToCreate)
    //{
    //    XElement productRoot = XmlTool.LoadListFromXMLElement(productPath); //get all the elements from the file

    //    //check if the customer exists in th file
    //    var productFromFile = (from product in productRoot.Elements()
    //                            where (product.Element("ID")?.Value == productToCreate.ID.ToString())
    //                            select product).FirstOrDefault();

    //    //throw an exception
    //    if (productFromFile != null)
    //        throw new DoubleFoundException("the product already exit");

    //    //add the customer to the root element
    //    productRoot.Add(
    //        new XElement("Product",
    //        new XElement("ID", productToCreate.ID),
    //        new XElement("Name", productToCreate.Name),
    //        new XElement("Artist", productToCreate.Artist),
    //        new XElement("Style", productToCreate.Categories),
    //        new XElement("AmountInStock", productToCreate.InStock),
    //        new XElement("Price", productToCreate.Price),
    //        new XElement("IsDeleted", productToCreate.IsDeleted),
    //        new XElement("Image", productToCreate.Image)))
    //        ;
    //    //save the root in the file
    //    XmlTool.SaveListToXMLElement(productRoot, productPath);
    //}
    #endregion

    #region DS Xml Files
    //string productPath = @"products.xml";
    //string orderPath = @"Order.xml";
    //string orderItemPath = @"OrderItem.xml";
    //string configPath = @"Config.xml";
    #endregion
    public IOrder Order { get; } = new Dal.Order();

    public IProduct Product { get; } = new Dal.Product();

    public IOrderItem OrderItem { get; } = new Dal.OrderItem();
}
