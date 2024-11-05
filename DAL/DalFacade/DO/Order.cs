using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DO;
/// <summary>
/// Structure fot orders 
/// </summary>
public struct Order
{
    /// <summary>
    /// Order ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Name of customer
    /// </summary>
    public string? CustomerName { get; set; }
    /// <summary>
    /// Customer email address
    /// </summary>
    public string? CustomerEmail { get; set; }
    /// <summary>
    /// Customer dwelling address
    /// </summary>
    public string? CustomerAddress { get; set; }
    /// <summary>
    /// The date the order was placed
    /// </summary>
    public DateTime? OrderDate { get; set; }
    /// <summary>
    /// Date the order was shipped out
    /// </summary>
    public DateTime? ShipDate { get; set; }
    /// <summary>
    /// Date customer recieved their order
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    /// <summary>
    /// Indicates whether the order has been deleted
    /// </summary>
    public bool IsDeleted { get; set; }
    /// <summary>
    /// Prints an Order
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}
