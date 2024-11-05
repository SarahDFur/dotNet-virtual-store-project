using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DO;
/// <summary>
/// Structure for the item in an order
/// </summary>
public struct OrderItem
{
    /// <summary>
    /// ID number of an item in an order
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// ID number of product
    /// </summary>
    public int ProductID { get; set; }
    /// <summary>
    /// Order ID/ serial number
    /// </summary>
    public int OrderID { get; set; }
    /// <summary>
    /// Price of one product
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// Amount of product ordered
    /// </summary>
    public int Amount { get; set; }
    /// <summary>
    /// Icon of item in an order
    /// </summary>
    public string? Image { get; set; }
    /// <summary>
    /// Indicates whether the order item has been deleted
    /// </summary>
    public bool IsDeleted { get; set; }
    /// <summary>
    /// Prints an Item in an Order
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
