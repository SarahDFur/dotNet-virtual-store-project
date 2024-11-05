using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class BlOrder : BlApi.IOrder
{
    readonly private static DalApi.IDal? dal = DalApi.Factory.Get();

    //---------------Converter functions----------------
    #region 

    private BO.OrderItem convertToBO(DO.OrderItem? item)
    {
        if(item != null)
        {
            try
            {
                return new BO.OrderItem()
                {
                    IdOfOrderItem = item?.ID ?? throw new BO.NullPropertyException("Null order-item ID"),
                    ProductID = item?.ProductID ?? throw new BO.NullPropertyException("Null order-item's product ID"),
                    ProductName = dal?.Product.GetById((int)item?.ProductID!).Name,
                    Price = item?.Price ?? throw new BO.NullPropertyException("Null order-item price"),
                    Amount = item?.Amount ?? 0,
                    TotalPrice = item?.Price * item?.Amount ?? 0,
                    Image = item?.Image,
                };
            }
            catch(DalApi.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException("The order item you wish to convert does not exist", ex);
            }
        }
        throw new BO.ObjectNotFoundException("The order item you wish to convert does not exist");
    }
   
    private BO.Order convertToBO(DO.Order? order)
    {
        if(order != null)
        {
            try
            {
                List<BO.OrderItem?> tempItems = makeCollection_ItemsInOrder(order);
                return new BO.Order()
                {
                    IdOfOrder = order?.ID ?? throw new BO.NullPropertyException("Null order ID"),
                    CustomerName = order?.CustomerName,
                    CustomerEmail = order?.CustomerEmail,
                    CustomerAddress = order?.CustomerAddress,
                    OrderStatus = calcOrderStatus(order),
                    OrderDate = order?.OrderDate,
                    ShipDate = order?.ShipDate,
                    DeliveryDate = order?.DeliveryDate,
                    Items = tempItems,
                    TotalPrice = tempItems.Sum(item => item?.TotalPrice ?? 0)
                };
            }
            catch(BO.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
            }
            catch(BO.DatesNotChronologicalException ex)
            {
                throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
            }
        }
        throw new BO.ObjectNotFoundException("The order you wish to convert does not exist");

    }
    
    private BO.OrderForList convertToOrderForList(DO.Order? order)
    {
        if(order != null)
        {
            try
            {
                List<BO.OrderItem?> tempItems = makeCollection_ItemsInOrder(order);
                return new BO.OrderForList()
                {
                    IdOfOrder = order?.ID ?? throw new BO.NullPropertyException("Null order ID"),
                    CustomerName = order?.CustomerName,
                    OrderStatus = calcOrderStatus(order),
                    AmountOfItems = tempItems.Count(),
                    TotalPrice = tempItems.Sum(item => item?.TotalPrice ?? 0)
                };
            }
            catch(BO.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
            }
            catch(BO.DatesNotChronologicalException ex)
            {
                throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
            }
        }
        throw new BO.ObjectNotFoundException("The order you wish to convert does not exist");
    }
    #endregion
    //---------------Helper functions----------------
    #region
    private BO.Enums.Status calcOrderStatus(DO.Order? order)
    {
        BO.Enums.Status temp;

        if (order?.OrderDate == null)
            throw new BO.DatesNotChronologicalException("Missing order date");
            
        temp = BO.Enums.Status.OrderConfirmed;

        if (order?.ShipDate != null)
            temp = (order?.ShipDate >= order?.OrderDate) ? 
                BO.Enums.Status.OrderSent : throw new BO.DatesNotChronologicalException("Ship date earlier than order date");

        if (order?.DeliveryDate != null)
        {
            if (order?.ShipDate == null) 
                throw new BO.DatesNotChronologicalException("Missing ship date");
            temp = (order?.ShipDate != null && order?.DeliveryDate >= order?.ShipDate) ?
                BO.Enums.Status.ProvidedToCustomer : throw new BO.DatesNotChronologicalException("Delivery date earlier than ship date");
        }
            

        return temp;
    }

    private List<BO.OrderItem?> makeCollection_ItemsInOrder(DO.Order? order)
    {
        try
        {
            IEnumerable<DO.OrderItem?>? items = dal?.OrderItem.GetAllItemsInOrder((int)order?.ID!);
            return (from DO.OrderItem ordItem in items! select convertToBO(ordItem)).ToList();
        }
        catch (DalApi.ObjectNotFoundException)
        {
            throw new BO.ObjectNotFoundException("Could not make collection of order items");
        }
        catch (BO.ObjectNotFoundException)
        {
            throw new BO.ObjectNotFoundException("Could not make collection of order items");
        }
    }
    #endregion
    //---------------Get functions----------------
    #region

    public BO.Order? GetOrder(int id)
    {
        try
        {
            DO.Order? tempOrder = dal?.Order.GetById(id);
            IEnumerable<DO.OrderItem?>? tempOrderItems = dal?.OrderItem.GetAllItemsInOrder(id);
            BO.Order? orderToReturn = convertToBO(tempOrder);
            return orderToReturn;
        }
        catch(DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Order does not exist", ex);
        }        
    }
    public IEnumerable<BO.OrderForList?> GetOrders()
    {
        try
        {
            IEnumerable<DO.Order?>? orders = dal?.Order.GetAll();//get all orders
            //return from DO.Order ord in orders! select convertToOrderForList(ord);//build the list of OrderForList
            return from ord in orders
                   let status = calcOrderStatus(ord)
                   let list = makeCollection_ItemsInOrder(ord)
                   select new BO.OrderForList()
                   {
                       IdOfOrder = ord?.ID ?? throw new BO.NullPropertyException("Null order ID"),
                       CustomerName = ord?.CustomerName,
                       OrderStatus = status,
                       AmountOfItems = list.Count(),
                       TotalPrice = list.Sum(item => item?.TotalPrice ?? 0)
                   };

        }
        catch (BO.DatesNotChronologicalException ex)
        {
            throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Order does not exist", ex);
        }
        catch(BO.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Order does not exist", ex);
        }
    }

    public IEnumerable<BO.Order?> GetOrdersForSimulator()
    {
        try
        {
            IEnumerable<DO.Order?>? orders = dal?.Order.GetAll();//get all orders
            return from ord in orders select convertToBO(ord);
        }
        catch (BO.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
        }
        catch (BO.DatesNotChronologicalException ex)
        {
            throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
        }
    }

    public IEnumerable<IGrouping<BO.Enums.Status, BO.OrderForList?>> GetAll_GroupedByOrderStatus()
    {
        try
        {
            IEnumerable<IGrouping<BO.Enums.Status, BO.OrderForList?>> orders =
            GetOrders().GroupBy(
                x => x!.OrderStatus,
             x => new BO.OrderForList()
             {
                 IdOfOrder = x!.IdOfOrder,
                 CustomerName = x.CustomerName,
                 OrderStatus = x.OrderStatus,
                 AmountOfItems = x.AmountOfItems,
                 TotalPrice = x.TotalPrice
             });
            return orders;
        }
        catch (BO.ObjectNotFoundException)
        {
            throw;
        }
    }

    public BO.OrderTracking? TrackOrder(int orderId)
    {
        try
        {
            DO.Order? tempOrder = dal?.Order.GetById(orderId);

            List<Tuple<DateTime?, string?>?> tempOrderTracking = new List<Tuple<DateTime?, string?>?>();
            tempOrderTracking.Add(new Tuple<DateTime?, string?>(tempOrder?.OrderDate, "Order confirmed"));

            if (tempOrder?.ShipDate != null)
            {                
                if (tempOrder?.ShipDate < tempOrder?.OrderDate)
                    throw new BO.DatesNotChronologicalException("Ship date earlier than order date");

                tempOrderTracking.Add(new Tuple<DateTime?, string?>(tempOrder?.ShipDate, "Order shipped"));
            }

            if (tempOrder?.DeliveryDate != null)
            {
                if (tempOrder?.OrderDate == null)
                    throw new BO.DatesNotChronologicalException("Missing order date");
                if (tempOrder?.ShipDate == null)
                    throw new BO.DatesNotChronologicalException("Missing ship date");
                if (tempOrder?.DeliveryDate < tempOrder?.ShipDate)
                    throw new BO.DatesNotChronologicalException("Delivery date earlier than ship date");
                
                tempOrderTracking.Add(new Tuple<DateTime?, string?>(tempOrder?.DeliveryDate, "Order deliverd"));
            }

            BO.OrderTracking resultOrderTracking = new BO.OrderTracking()
            {
                IdOfOrder = orderId,
                OrderStatus = calcOrderStatus(tempOrder),
                Tracking = tempOrderTracking
            };
            return resultOrderTracking;

        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Cannot track order - Order does not exist", ex);
        }
        catch (BO.DatesNotChronologicalException ex)
        {
            throw new BO.ObjectNotFoundException("The order you wish to convert does not exist", ex);
        }
    }
    #endregion
    //---------------Update data functions----------------
    #region
    public BO.Order? UpdateShipDate(int orderId)
    {
        DO.Order? dOrder;
        BO.Order bOrder;
        try
        {
            dOrder = dal?.Order.GetById(orderId);
            bOrder = GetOrder(orderId)!;
        }
        catch(DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Order does not exist", ex);
        }
        DO.Order dOrder_helper = (DO.Order)dOrder!;

        if (dOrder_helper.ShipDate != null)
            throw new BO.DoubleFoundException("Shippment date already exist");

        dOrder_helper.ShipDate = DateTime.Now;
        bOrder.ShipDate = DateTime.Now;
        bOrder.OrderStatus = BO.Enums.Status.OrderSent;

        try
        {
            dal?.Order.Update(dOrder_helper);
        }
        catch(DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Unable to update order", ex);
        }
        return bOrder;
    }
    public BO.Order? UpdateDeliveryDate(int orderId)
    {
        DO.Order? dOrder;
        BO.Order bOrder;
        try
        {
            dOrder = dal?.Order.GetById(orderId);
            bOrder = GetOrder(orderId)!;
        }
        catch(DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Order does not exist", ex);
        }
        DO.Order dOrder_helper = (DO.Order)dOrder!;

        if (dOrder_helper.ShipDate == null) 
            throw new BO.DatesNotChronologicalException("order wasn't shipped yet, can't update delivery date");
        if (dOrder_helper.DeliveryDate != null)
            throw new BO.DoubleFoundException("Delivery date already exist");

        dOrder_helper.DeliveryDate = DateTime.Now;
        bOrder.DeliveryDate = DateTime.Now;
        bOrder.OrderStatus = BO.Enums.Status.ProvidedToCustomer;

        try
        {
            dal?.Order.Update(dOrder_helper);
        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("Unable to update order", ex); 
        }
        return bOrder;
    }

    public BO.Order? UpdateOrder(int orderId, int productId, int newAmount)
    {
        //check order exist
        DO.Order? dOrder;
        BO.Order bOrder;
        try
        {
            dOrder = dal?.Order.GetById(orderId);
            bOrder = GetOrder(orderId)!;
        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("no order with this ID", ex);
        }
        //check order wasn't shipped or delivered aleardy
        if (bOrder.OrderStatus == BO.Enums.Status.OrderSent)
            throw new BO.DatesNotChronologicalException("order has already been sent");
        if (bOrder.OrderStatus == BO.Enums.Status.ProvidedToCustomer)
            throw new BO.DatesNotChronologicalException("order has already been delivered");
        //check product exist in general
        DO.Product? dProduct;
        try
        {
            dProduct = dal?.Product.GetById(productId);
        }
        catch (DalApi.ObjectNotFoundException ex)
        {
            throw new BO.ObjectNotFoundException("no product with this ID", ex);
        }
        //check new amount is non-negative
        if (newAmount < 0) 
            throw new BO.FormatIsIncorrectException("Cannot update to negative amount");

        //-------------------------------------------------
        //check whether product exist in order (items list)
        DO.OrderItem? dOrderItem;
        BO.OrderItem bOrderItem;
        try
        {
            //check if item with ID number and similar price already exists in order. if so, update it.
            //if not, throw exception to jump to adding a new order item to order 
            IEnumerable<DO.OrderItem?>? dOrderItems_List = dal?.OrderItem.GetAllItemsInOrder(orderId);
            dOrderItem = dOrderItems_List?.FirstOrDefault(x => (x?.ProductID == productId) && (x?.Price == dProduct?.Price));
            if (dOrderItem == null)
                throw new DalApi.ObjectNotFoundException();
            bOrderItem = convertToBO(dOrderItem);            

            if (newAmount == dOrderItem?.Amount)
                return bOrder;
            if (newAmount == 0)
            {
                //update BO entity
                int i = bOrder.Items!.FindIndex(x => x?.ProductID == productId);
                bOrder.Items.RemoveAt(i);
                bOrder.TotalPrice -= bOrderItem.TotalPrice;
                //update DO entities
                DO.Product product_helper = (DO.Product)dProduct!;
                product_helper.InStock += (int)dOrderItem?.Amount!;                
                try
                {
                    dal?.OrderItem.Delete((int)dOrderItem?.ID!);
                    dal?.Product.Update(product_helper);
                }
                catch(DalApi.ObjectNotFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
                }
                catch(DalApi.DoubleFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
                }
                return bOrder;
            }
            if (newAmount < dOrderItem?.Amount)
            {
                int diffAmount = (int)dOrderItem?.Amount! - newAmount;
                //update BO entities
                bOrder.TotalPrice -= diffAmount * bOrderItem.Price;
                int i = bOrder.Items!.FindIndex(x => x?.ProductID == productId);
                bOrder.Items.RemoveAt(i);
                bOrderItem.Amount = newAmount;
                bOrderItem.TotalPrice -= diffAmount * bOrderItem.Price;
                bOrder.Items!.Add(bOrderItem);
                //update DO entites
                DO.OrderItem orderitem_helper = (DO.OrderItem)dOrderItem!;
                orderitem_helper.Amount = newAmount;
                DO.Product product_helper = (DO.Product)dProduct!;
                product_helper.InStock -= (int)dOrderItem?.Amount!;
                try
                {
                    dal?.OrderItem.Update(orderitem_helper);
                    dal?.Product.Update(product_helper);
                }
                catch(DalApi.ObjectNotFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
                }
                catch(DalApi.DoubleFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
                }
            }
            else //(newAmount > dOrderItem?.Amount)
            {
                int addedAmount = newAmount - (int)dOrderItem?.Amount!;
                if (addedAmount > dProduct?.InStock)
                    throw new BO.ObjectStockOverflowException($"Product {productId} doesn't have enough in stock, cannot update amount in order");
                //update BO entities
                bOrder.TotalPrice += addedAmount * bOrderItem.Price;
                int i = bOrder.Items!.FindIndex(x => x?.ProductID == productId);
                bOrder.Items.RemoveAt(i);
                bOrderItem.Amount = newAmount;
                bOrderItem.TotalPrice += addedAmount * bOrderItem.Price;
                bOrder.Items.Add(bOrderItem);
                //update Do entities
                DO.OrderItem orderitem_helper = (DO.OrderItem)dOrderItem!;
                orderitem_helper.Amount = newAmount;
                DO.Product product_helper = (DO.Product)dProduct!;
                product_helper.InStock -= (int)dOrderItem?.Amount!;
                try
                {
                    dal?.OrderItem.Update(orderitem_helper);
                    dal?.Product.Update(product_helper);
                }
                catch (DalApi.ObjectNotFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
                }
                catch (DalApi.DoubleFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
                }
            }
            return bOrder;
        }
        catch (DalApi.ObjectNotFoundException)
        {
            //if exception was thrown, item with ID number doesn't exist in order, or exists but price has changed
            //check newAmount
            if (newAmount == 0)
                return bOrder;
            //check stock
            if (newAmount > dProduct?.InStock)
                throw new BO.ObjectStockOverflowException($"Product {productId} doesn't have enough in stock, cannot add to order");
            //update DO entities
            int? new_orderItem_id;
            DO.Product product_helper = (DO.Product)dProduct!;
            product_helper.InStock -= newAmount;
            try
            {
                dal?.Product.Update(product_helper);
                new_orderItem_id = dal?.OrderItem.Add(new DO.OrderItem()
                {
                    ID = 0,
                    OrderID = orderId,
                    ProductID = productId,
                    Price = (double)dProduct?.Price!,
                    Amount = newAmount,
                    Image = dProduct?.Image,
                    IsDeleted = false
                });
            }
            catch (DalApi.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
            }
            catch (DalApi.DoubleFoundException ex)
            {
                throw new BO.ObjectNotFoundException($"failed to update order {orderId}", ex);
            }
            //update BO entities            
            bOrder.Items!.Add(new BO.OrderItem()
            {
                IdOfOrderItem = (int)new_orderItem_id!,
                ProductID = productId,
                ProductName = dProduct?.Name,
                Price = (int)dProduct?.Price!,
                Amount = newAmount,
                TotalPrice = newAmount * (int)dProduct?.Price!,
                Image = dProduct?.Image
            });
            bOrder.TotalPrice += newAmount * (int)dProduct?.Price!;

            return bOrder;
        }

        #region
        //  if  -
        //      if newAmount <= stock
        //          1)make new DO.orderItem in DS.orderItemsList,
        //          2)update amount in DS.productsList with differnce
        //          3)make new BO.orderItem in BO.order.items,
        //      if newAmount > productStock
        //          throw exc-not-enough with message of current stock


        //  if exist in order - 
        //      if (newAmount > oldAmount) && (newAmount-oldAmount > inStock)
        //          throw exc-not-enough with message of current stock
        //      else
        //          1)update amount + total in BO.orderItem in BO.order.items,
        //          2)update total in BO.order.total,
        //          3)update existing DO.orderItem in DS.orderItemsList,
        //          4)update amount in DS.productsList with differnce
        //

        //if (newAmount > dOrderItem.Amount && (newAmount - dOrderItem.Amount) > dProduct.InStock)
        //{
        //    throw new Exception();//!!!!!!!!!!! exc-not-enough with message of current stock

        //}
        //else
        //{
        //    bOrderItem.Amount = newAmount;
        //    dOrderItem.Amount = newAmount;
        //}


        //if (newAmount > dOrderItem.Amount)
        //    if (newAmount <= dProduct.InStock)
        //    {
        //        bOrderItem.Amount = newAmount;
        //        dOrderItem.Amount = newAmount;
        //        bOrder.Items.Append(bOrderItem);
        //        dal.OrderItem.Update(dOrderItem);

        //        dProduct.InStock -= newAmount;
        //        dal.Product.Update(dProduct);
        //    }
        //    else
        //    {
        //        throw new Exception();//!!!!!!!!!!
        //    }



        //      if newAmount > oldAmount
        //          if newAmount <= productStock 
        //              update amount in property "items"
        //              + decrease stock with difference
        //              + update amount in DataSource-orderItems-list
        //              + update amount in DataSource-products-list
        //          if newAmount > productStock 
        //              throw exc-not-enough with message of current stock
        //      if newAmount <= old amount
        //          update amount in property "items"
        //          + increase stock with difference 
        //          + update amount in DataSource-orderItems-list
        //          + update amount in DataSource-products-list
        #endregion
    }

    public int AddOrderForSimulator()
    {
        int? id = dal?.Order.Add(new DO.Order()
        {
            ID = 0,
            CustomerName = "Yael",
            CustomerAddress = "Jerusalem",
            CustomerEmail = "yael@gmail.com",
            OrderDate = DateTime.Now,
            ShipDate = null,
            DeliveryDate = null,
            IsDeleted = false
        });
        return id ?? -1;
    }
    #endregion
}
