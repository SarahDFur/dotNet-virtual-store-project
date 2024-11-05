using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class OrderTracking
{
    /// <summary>
    /// Order ID
    /// </summary>
    public int IdOfOrder { get; set; }
    /// <summary>
    /// Status of the order
    /// </summary>
    public Enums.Status OrderStatus { get; set; }
    /// <summary>
    /// list to keep track of a delivery
    /// </summary>
    public List<Tuple<DateTime?, string?>?>? Tracking { set; get; }
    /// <summary>
    /// print OrderTracking
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}