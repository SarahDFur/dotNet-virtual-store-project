using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BO;
namespace BlApi;

public interface IOrder
{
    /// <summary>
    /// Get list of orders - for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<OrderForList?> GetOrders();
    /// <summary>
    /// Get list of orders - for simulator
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Order?> GetOrdersForSimulator();
    /// <summary>
    /// add order to simulator
    /// </summary>
    /// <returns></returns>
    public int AddOrderForSimulator();
    /// <summary>
    /// Get order from list - for manager and catalog
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Order? GetOrder(int id);
    /// <summary>
    /// Get list of orders grouped by order status - for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<BO.Enums.Status, BO.OrderForList?>> GetAll_GroupedByOrderStatus();
    /// <summary>
    /// Update the shipment date of an order - for manager
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public Order? UpdateShipDate(int orderId);
    /// <summary>
    /// Update the delivery date of an order - for manager
    /// </summary>
    /// <param name="orderId"></param>
    public Order? UpdateDeliveryDate(int orderId);
    /// <summary>
    /// Returns a list - tracking - for manager
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public OrderTracking? TrackOrder(int orderId);
    /// <summary>
    /// Bonus - Update order in list - for manager
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public Order? UpdateOrder(int orderId, int productId, int amount);
}