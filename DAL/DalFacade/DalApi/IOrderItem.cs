using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface IOrderItem : ICrud<OrderItem>
{
    public IEnumerable<OrderItem?> GetAllItemsInOrder(int orderNumber);

    //מתודה שמקבלת מספר יחיד, שהוא מספר ההזמנה, ומחזירה רשימה של כל הפריטים באותה הזמנה.


    public OrderItem GetItem(int orderNumber, int productNumber);

        //מתודה שתקבל שתקבל שני נתונים - מספר הזמנה ומספר מוצר - ותחזיר את הפריט המתאים(כלומר – שהחיפוש לא יהיה לפי המפתח – אלא לפי הזמנה ומוצר).

 
}