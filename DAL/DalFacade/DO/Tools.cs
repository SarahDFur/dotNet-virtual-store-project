using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DO
{
    /// <summary>
    /// Prints the objects
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Prints "T", no matter which type of object it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToStringProperty<T>(this T t) 
        {
            string str = "";
            foreach (PropertyInfo item in t!.GetType().GetProperties())
                if (item.Name != "IsDeleted")
                    str += $"\n{item.Name}: {item.GetValue(t, null)}";                
            return str;
        }
    }
}
