using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BO;
namespace BlApi;
/// <summary>
/// cart operations - only for customer
/// </summary>
public interface ICart
{
    /// <summary>
    /// Add an item to cart
    /// </summary>
    /// <param name="c"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Cart? AddProductToCart(Cart c, int pId);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <param name="orderId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public Cart? UpdateAmountOfProductInCart(Cart c, int ProductId, int amount);
    /// <summary>
    /// Pay for the products in cart
    /// </summary>
    /// <param name="c"></param>
    /// <param name="name"></param>
    /// <param name="mail"></param>
    /// <param name="address"></param>
    public int CartPayment(Cart c);
}