using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public static class Tools
    {
        /// <summary>
        /// Prints "T", no matter which type of object it is
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToStringProperty<T>(this T t, string suffix = "")
        {
            string str = "";
            foreach (PropertyInfo item in t!.GetType().GetProperties())
            {

                var value = item.GetValue(t, null);
                if (value is string)
                    str += suffix + $"{item.Name}: {item.GetValue(t, null)}";
                else
                {
                    if (value is IEnumerable)
                    {
                        str += $" {item.Name}: ";
                        foreach (var item2 in (IEnumerable)value)
                            str += item2.ToStringProperty("  ");
                    }
                    else
                        str += suffix + $" {item.Name}: {item.GetValue(t, null)} ";
                }                
            }
            //str += "\n";
            return str;
        }   
        //public static string ToStringProperty<T>(this T t, string suffix = "")
        //{
        //    string str = "";
        //    foreach (PropertyInfo item in t!.GetType().GetProperties())
        //    {

        //        var value = item.GetValue(t, null);
        //        if (value is string)
        //            str += "\n" + suffix + $"{item.Name}: {item.GetValue(t, null)}";
        //        else
        //        {
        //            if (value is IEnumerable)
        //            {
        //                str += $"\n{item.Name}: ";
        //                foreach (var item2 in (IEnumerable)value)
        //                    str += item2.ToStringProperty("  ");
        //            }
        //            else
        //                str += "\n" + suffix + $"{item.Name}: {item.GetValue(t, null)}";
        //        }                
        //    }
        //    str += "\n";
        //    return str;
        //}
    }
}
