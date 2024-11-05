using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Order
{
    /// <summary>
    /// Order ID
    /// </summary>
    public int IdOfOrder { get; set; }
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
    public Enums.Status OrderStatus { get; set; }
    /// <summary>
    /// The date of payment completion 
    /// </summary>
    public DateTime? OrderDate { get; set; }
    /// <summary>
    /// Status of the order
    /// </summary>
    public DateTime? ShipDate { get; set; }
    /// <summary>
    /// Date customer recieved their order
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    /// <summary>
    /// List of items ordered
    /// </summary>
    public List<OrderItem?>? Items { get; set; }
    /// <summary>
    /// The total price of the order
    /// </summary>
    public double TotalPrice { get; set; }
    /// <summary>
    /// print Order
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}