using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BlApi;
using DalApi;

namespace BlImplementation;
//checks:
//1.amount smaller than amount in stock (in DO)
//2.there are no emails that are the same
internal class BlCart : BlApi.ICart
{
    readonly private static DalApi.IDal? dal = DalApi.Factory.Get();
    
    private static bool validEmailFormat(string? email)
    {
        if (email == null) 
            return true;
        if (email.Any(x => x == ' ')) 
            return false;
        if (email.Contains('@') && (email.EndsWith(".com") || email.EndsWith(".co.il")))
            return true;
        return false;
    }

    public BO.Cart? AddProductToCart(BO.Cart cart, int pId)
    {
        if (cart.items == null || cart.items?.Any(x => x?.ProductID == pId) == false) //no product with this id in cart yet
        {
            try
            {
                DO.Product? DoProduct_ToAddToCart = dal?.Product.GetById(pId);
                if (DoProduct_ToAddToCart?.InStock > 0 ) //if the product is in stock
                {
                    BO.OrderItem BoOrderItem_ToAdd = new BO.OrderItem();
                    //--------update into BO.OrderItem---------
                    BoOrderItem_ToAdd.IdOfOrderItem = 0;
                    BoOrderItem_ToAdd.ProductID = pId;
                    BoOrderItem_ToAdd.ProductName = DoProduct_ToAddToCart?.Name;
                    BoOrderItem_ToAdd.Price = DoProduct_ToAddToCart?.Price ?? 0;
                    BoOrderItem_ToAdd.Amount = 1;
                    BoOrderItem_ToAdd.TotalPrice = DoProduct_ToAddToCart?.Price ?? 0;
                    BoOrderItem_ToAdd.Image = DoProduct_ToAddToCart?.Image;
                    //--------------add to cart----------------
                    cart.items ??= new List<BO.OrderItem?>(); //if (cart.items == null) cart.items = new List<BO.OrderItem?>();
                    cart.items.Add(BoOrderItem_ToAdd);
                    cart.TotalPrice += BoOrderItem_ToAdd.Price;
                    
                    return cart;
                }
                else
                    throw new BO.ObjectStockOverflowException($"Product {pId} not in stock, cannot add to cart");
            }
            catch(DalApi.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException($"Product {pId} does not exist", ex);
            }
        }

        else //product with this id already exists in cart
        {
            try
            {
                DO.Product? DoProduct_ToAddToAmountInCart = dal?.Product.GetById(pId);
                int index = cart.items?.FindIndex(x => x?.ProductID == pId) ?? -1;
                int availableStock = DoProduct_ToAddToAmountInCart?.InStock - cart.items![index]!.Amount ?? 0; //we sure we can use ! opperator

                if ( availableStock > 0 )//there is enough of product in stock, and product is in the cart
                {
                    cart.items[index]!.Amount++;
                    cart.items[index]!.TotalPrice += DoProduct_ToAddToAmountInCart?.Price ?? 0;
                    cart.TotalPrice += DoProduct_ToAddToAmountInCart?.Price ?? 0;

                    return cart;
                }
                else
                    throw new BO.ObjectStockOverflowException($"Product {pId} not in stock anymore, cannot add more to cart");
            }
            catch (DalApi.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException($"Product {pId} doesn't exist", ex);
            }
        }
    }

    public BO.Cart? UpdateAmountOfProductInCart(BO.Cart cart, int productId, int updatedAmount)
    {
        int index_for_check = cart.items?.FindIndex(x => x?.ProductID == productId) ?? -1;
        if (index_for_check == -1)
            throw new BO.ObjectNotFoundException($"Product {productId} doesn't exist in cart, cannot update. Use AddProductToCart method");
        if (updatedAmount < 0)
            throw new BO.FormatIsIncorrectException("Cannot update to negative amount");

        int oldAmount = cart.items?[index_for_check]!.Amount ?? 0;

        if (updatedAmount == 0)
        {
            cart.TotalPrice -= cart.items![index_for_check]!.TotalPrice;
            cart.items.RemoveAt(index_for_check);//we sure won't throw outOfRange
            return cart;
        }

        if (updatedAmount < oldAmount)
        {
            cart.items![index_for_check]!.Amount = updatedAmount;
            cart.items[index_for_check]!.TotalPrice -= (oldAmount - updatedAmount) * cart.items[index_for_check]!.Price;
            cart.TotalPrice -= (oldAmount - updatedAmount) * cart.items[index_for_check]!.Price;
            return cart;
        }

        else // updatedAmount > oldAmount
        {
            try
            {
                DO.Product? DoProduct_ToAddToAmountInCart = dal?.Product.GetById(productId);
                int availableStock = DoProduct_ToAddToAmountInCart?.InStock - cart.items![index_for_check]!.Amount ?? 0; //we sure we can use ! opperator
                if (availableStock >= updatedAmount - oldAmount)
                {
                    cart.items[index_for_check]!.Amount = updatedAmount;
                    cart.items[index_for_check]!.TotalPrice += (updatedAmount - oldAmount) * cart.items[index_for_check]!.Price;
                    cart.TotalPrice += (updatedAmount - oldAmount) * cart.items[index_for_check]!.Price;
                    return cart;
                }
                else
                    throw new BO.ObjectStockOverflowException($"Product {productId} doesn't have enough in stock, cannot update amount in cart");

            }
            catch (DalApi.ObjectNotFoundException ex)
            {
                throw new BO.ObjectNotFoundException($"Product {productId} doesn't exist", ex);
            }
        }
    }

    public int CartPayment(BO.Cart cart)
    {
        //בדיקת תקינות
        //1/שם וכתובת קונה לא ריקים
        //2/כתובת דוא"ל ריקה או לפי פורמט חוקי
        if (cart.CustomerName == null)
            throw new BO.FormatIsIncorrectException("Missing customer name");
        if (cart.CustomerAddress == null)
            throw new BO.FormatIsIncorrectException("Missing customer address");
        if (cart.CustomerEmail == null)
            throw new BO.FormatIsIncorrectException("Missing customer email");        
        if (!validEmailFormat(cart.CustomerEmail))
            throw new BO.FormatIsIncorrectException("Customer email is in incorrect format");
        if (cart.items == null)
            throw new BO.FormatIsIncorrectException("Cart is empty, cannot checkout");
        //בדיקת תקינות
        //1/כל המוצרים קיימים
        //2/כמויות חיוביות
        //3/מחירים חיוביים
        //4/יש מספיק במלאי
        foreach (BO.OrderItem? t in cart.items)
        {
            if(t != null)
            {
                try
                {
                    DO.Product? p = dal?.Product.GetById(t.ProductID);
                    if (t.Price <= 0)
                    {
                        throw new BO.FormatIsIncorrectException($"Product {p?.ID} is in cart with zero or negative PRICE, need to be removed");
                    }
                    if (t.Amount <= 0)
                    {
                        throw new BO.FormatIsIncorrectException($"Product {p?.ID} is in cart with zero or negative AMOUNT, need to be removed");
                    }
                    if (p?.InStock < t.Amount)
                    {
                        throw new BO.ObjectStockOverflowException($"Product {p?.ID} doesn't have enough in stock");
                    }
                }
                catch (DalApi.ObjectNotFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"Product with ID {t.ProductID} doesn't exist in catalog", ex);
                }
            }            
        }

        //-------add new DO order--------
        int new_order_id;
        try
        {            
            DO.Order new_order = new DO.Order()
            {
                ID = 0,
                CustomerName = cart.CustomerName,
                CustomerAddress = cart.CustomerAddress,
                CustomerEmail = cart.CustomerEmail,
                OrderDate = DateTime.Now,
                ShipDate = null,
                DeliveryDate = null,
                IsDeleted = false
            };
            new_order_id = dal?.Order.Add(new_order) ?? -1;
        }
        catch(DalApi.DoubleFoundException ex)
        {
            throw new BO.DoubleFoundException("Cannot make a new order", ex);
        }

        foreach(BO.OrderItem? t in cart.items)
        {
            if (t != null)
            {
                //-------add new DO orderItem---------
                try
                {
                    DO.OrderItem new_order_item = new DO.OrderItem()
                    {
                        ID = 0,
                        OrderID = new_order_id,
                        ProductID = t.ProductID,
                        Price = t.Price,
                        Amount = t.Amount,
                        Image = t.Image,
                        IsDeleted = false
                    };
                    dal?.OrderItem.Add(new_order_item);
                }
                catch (DalApi.DoubleFoundException ex)
                {
                    throw new BO.DoubleFoundException("Cannot make a new order item", ex);
                }
                //-------update amount of DO product---------
                try
                {
                    DO.Product? p = dal?.Product.GetById(t.ProductID);
                    DO.Product p_helper = (DO.Product)p!;
                    p_helper.InStock -= t.Amount;
                    dal?.Product.Update(p_helper);
                }
                catch (DalApi.ObjectNotFoundException ex)
                {
                    throw new BO.ObjectNotFoundException($"Product with ID {t.ProductID} doesn't exist in catalog", ex);
                }
                catch (DalApi.DoubleFoundException ex)
                {
                    throw new BO.DoubleFoundException($"Product {t.ProductID} failed to update amount", ex);
                }
            }        
        }
        return new_order_id;
    }
}