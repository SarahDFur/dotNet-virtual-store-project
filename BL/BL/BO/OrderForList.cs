using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class OrderForList
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
    /// Status of the order
    /// </summary>
    public Enums.Status OrderStatus { get; set; }
    /// <summary>
    /// Amount of items in order
    /// </summary>
    public int AmountOfItems { get; set; }
    /// <summary>
    /// Total price of orders
    /// </summary>
    public double TotalPrice { get; set; }
    /// <summary>
    /// print OrderForList
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}