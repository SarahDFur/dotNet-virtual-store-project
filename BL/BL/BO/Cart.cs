using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Cart
{
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
    /// List of items ordered
    /// </summary>
    public List<OrderItem?>? items { get; set; }
    /// <summary>
    /// Total price of everything we wish to buy (collective price of all items ordered)
    /// </summary>
    public double TotalPrice { get; set; }
    /// <summary>
    /// prints Cart
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}