using DalApi;
using DO;
using System.ComponentModel;

namespace Dal;

internal sealed class DalList : IDal
{
    #region singleton
    class Nested
    {
        public Nested() { }   
        public static readonly IDal instance = new DalList(); 
    }

   
    static DalList() { }
    DalList() { }
    public static IDal Instance { get { return Nested.instance; } }
    #endregion

    public IOrder Order { get; } = new Dal.DalOrder();

    public IProduct Product { get; } = new Dal.DalProduct();

    public IOrderItem OrderItem { get; } = new Dal.DalOrderItem();
}