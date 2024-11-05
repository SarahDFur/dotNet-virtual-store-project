using BO;
using PL.PO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace PL;

public class Castings
{
    /// <summary>
    /// converts IEnumerable of product for list from BL to ObservableCollection of product for list from PL
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    public static ObservableCollection<PO.ProductForList?>? ProductForList_ConvertIEnumerableToObservable(IEnumerable<BO.ProductForList?> products)
    {
        ObservableCollection<PO.ProductForList?> products1 = new();
        if (products != null) 
        {
            foreach (BO.ProductForList? prod in products)
            {
                PO.ProductForList p = new()
                {
                    ID = prod?.ID ?? 0,
                    Title = prod?.NameOfProduct,
                    Artist = prod?.Artist,
                    Price = prod?.Price ?? 0,
                    Image = prod?.Image
                };
                switch (prod?.Categories)
                {
                    case BO.Enums.ArtStyles.Realism:
                        p.Style = ArtStyles.Realism; break;
                    case BO.Enums.ArtStyles.Cubism:
                        p.Style = ArtStyles.Cubism; break;
                    case BO.Enums.ArtStyles.SemiRealism:
                        p.Style = ArtStyles.SemiRealism; break;
                    case BO.Enums.ArtStyles.Abstract:
                        p.Style = ArtStyles.Abstract; break;
                    case BO.Enums.ArtStyles.Cartoon:
                        p.Style = ArtStyles.Cartoon; break;
                    case BO.Enums.ArtStyles.None:
                        p.Style = ArtStyles.None; break;
                    case null:
                        p.Style = ArtStyles.None; break;
                }
                products1.Add(p);
            }
            return products1;
        }
        return null;
    }

    public static ObservableCollection<PO.OrderForList?> OrderForList_ConvertIEnumerableToObservable(IEnumerable<BO.OrderForList?> orders)
    {
        ObservableCollection<PO.OrderForList?> orders1 = new();
        foreach (BO.OrderForList? ord in orders)
        {
            PO.OrderForList o = new()
            {
                ID = ord?.IdOfOrder ?? 0,
                CustomerName = ord?.CustomerName,
                Amount = ord?.AmountOfItems ?? 0,
                TotalPrice = ord?.TotalPrice ?? 0
            };
            switch (ord?.OrderStatus)
            {
                case BO.Enums.Status.OrderConfirmed:
                    o.OrderStatus = Status.OrderConfirmed; break;
                case BO.Enums.Status.OrderSent:
                    o.OrderStatus = Status.OrderSent; break;
                case BO.Enums.Status.ProvidedToCustomer:
                    o.OrderStatus = Status.ProvidedToCustomer; break;
                case BO.Enums.Status.None:
                    o.OrderStatus = Status.None; break;
                case null:
                    o.OrderStatus = Status.None; break;
            }
            orders1.Add(o);
        }
        return orders1;
    }

    public static ObservableCollection<PO.ProductItem> ProductItem_ConvertIEnumerableToObservable(IEnumerable<BO.ProductItem?> productItems)
    {
        ObservableCollection<PO.ProductItem> POproductItems = new();
        foreach (BO.ProductItem? p in productItems)
        {
            POproductItems.Add(new PO.ProductItem()
            {
                ID = p!.ID,
                Title = p.NameOfProduct,
                Artist = p.Artist,
                Style = (PO.ArtStyles)p.Categories,
                Price = p.Price,
                Amount = p.AmountInCart,
                Stocked = p.InStock,
                Image=p.Image
            });
        }

        return POproductItems;
    }

    public static ObservableCollection<PO.Order?> Order_ConvertIEnumerableToObservable(IEnumerable<BO.Order?> orders)
    {
        ObservableCollection<PO.Order?> newOrders = new();
        foreach(BO.Order? o in orders)
        {
            newOrders.Add(new PO.Order()
            {
                ID = o?.IdOfOrder ?? 0,
                CustomerName = o?.CustomerName ?? "",
                CustomerAddress = o?.CustomerAddress ?? "",
                CustomerEmail = o?.CustomerEmail ?? "",
                OrderDate = o?.OrderDate ?? DateTime.MinValue,
                ShipDate = o?.ShipDate ?? DateTime.MinValue,
                DeliveryDate = o?.DeliveryDate ?? DateTime.MinValue,
                OrderStatus = (PO.Status)o?.OrderStatus!,
                TotalPrice = o.TotalPrice,
                Items = OrderItemList_convertPoToBo(o.Items)
            });
        }
        return newOrders;
    }

    public static List<PO.OrderItem?> OrderItemList_convertPoToBo(List<BO.OrderItem?>? bList)
    {
        List<PO.OrderItem?> pList = new();
        if (bList != null)
        {
            pList = (from oi in bList
                     select new PO.OrderItem()
                     {
                         ID = oi.IdOfOrderItem,
                         ProductID = oi.ProductID,
                         ProductName = oi.ProductName,
                         Price = oi.Price,
                         TotalPrice = oi.TotalPrice,
                         Amount = oi.Amount,
                         Image = oi.Image
                     }).ToList();
        }
        return pList;
    }

    public static ObservableCollection<PO.OrderItem?>? Orderitem_Items_ConvertIEnumerableToObservable(List<PO.OrderItem?> items)
    {
        ObservableCollection<PO.OrderItem?>? orderItems = new();
        foreach (var o_it in items)
            orderItems.Add(o_it);
        return orderItems;
    }

    
    //public static ObservableCollection<PO.OrderItem> OrderItem_ConvertIEnumerableToObservable(IEnumerable<BO.OrderItem?> orderItems)
    //{
    //    ObservableCollection<PO.OrderItem> POorderItems = new();
    //    foreach (BO.OrderItem? p in orderItems)
    //    {
    //        POorderItems.Add(new PO.OrderItem()
    //        {
    //            ID = p!.IdOfOrderItem,
    //            ProductID = p!.ProductID,
    //            ProductName = p!.ProductName,
    //            Price = p.Price,
    //            Amount = p.Amount,
    //            TotalPrice = p.TotalPrice,
    //            Image = p.Image
    //        });
    //    }
    //    return POorderItems;
    //}   

    //public static BO.Cart Cart_convertPo_to_Bo(PO.Cart cart)
    //{
    //    BO.Cart bCart = new()
    //    {
    //        CustomerName = cart.Name,
    //        CustomerEmail = cart.Email,
    //        CustomerAddress = cart.Address,
    //        TotalPrice = cart.TotalPrice,
    //        items = OrderItemList_convertPoToBo(cart.Items)
    //    };
    //    return bCart;
    //}


}