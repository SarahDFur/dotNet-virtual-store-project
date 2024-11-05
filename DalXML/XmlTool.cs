using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;
using DalApi;
using DO;
using System.Xml.Serialization;

static internal class XmlTool
{
    const string suffixPath = @"..\xml\";
    #region Extension Fuctions
    public static T? ToEnumNullable<T>(this XElement element, string name) where T : struct, Enum =>
        Enum.TryParse<T>((string?)element.Element(name), out var result) ? (T?)result : null;

    public static DateTime? ToDateTimeNullable(this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;

    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;
   public static bool? ToBoolNullable(this XElement element, string name) =>
       bool.TryParse((string?)element.Element(name), out var result) ? (bool?)result : null;

    public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;
    #endregion
    #region XElement
    #region Load
    public static XElement LoadListFromXMLElement(string filePath)
    {
        //string filePathIs = $"{suffixPath + filePath}";
        //try
        //{
        //    if (File.Exists(filePathIs))
        //        return XElement.Load(filePathIs);
        //    XElement rootElem = new(filePathIs);
        //    rootElem.Save(filePathIs);
        //    return rootElem;
        //}
        //catch (Exception ex)
        //{
        //    //new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {dir + filePath}", ex);
        //    throw new Exception($"fail to load xml file: {filePathIs}", ex);
        //}
        try
        {
            if (File.Exists(suffixPath + filePath))
            {
                return XElement.Load(suffixPath + filePath);
            }
            else
            {
                XElement rootElem = new(filePath);
                if (filePath == @"Config.xml") { }
                rootElem.Save(suffixPath + filePath);
                return rootElem;
            }
        }
        catch (Exception ex)
        {
            throw new DalApi.LoadingException(filePath, $"fail to load xml file: {filePath}", ex);
        }
    }
    #endregion
    #region Save
    public static void SaveListToXMLElement(XElement rootElem, string filePath)
    {
        try
        {
            rootElem.Save(suffixPath + filePath);
        }
        catch (Exception ex)
        {
            throw new LoadingException(suffixPath + filePath, $"fail to create xml file: {suffixPath + filePath}", ex);
        }
    }
    #endregion
    #endregion

    #region XmlSerializer
    //save a complete list in a specific file- throw exception in case of problems..
    //for the using with XMLSerializer..
    public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
    {
        try
        {
            FileStream file = new(suffixPath + filePath, FileMode.Create);
            XmlSerializer x = new(list.GetType());
            x.Serialize(file, list);
            file.Close();
        }
        catch (Exception ex)
        {
            throw new LoadingException(suffixPath + filePath, $"fail to create xml file: {suffixPath + filePath}", ex);
        }
    }

    //load a complete list from a specific file- throw exception in case of problems..
    //for the using with XMLSerializer..
    public static List<T?>? LoadListFromXMLSerializer<T>(string filePath)
    {
        try
        {
            if (File.Exists(suffixPath + filePath))
            {
                List<T?>? list;
                XmlSerializer x = new (typeof(List<T>));
                FileStream file = new (suffixPath + filePath, FileMode.Open);
                list = (List<T?>?)x!.Deserialize(file)!;
                file.Close();
                return list!;
            }
            else
                return new List<T?>();
        }
        catch (Exception ex)
        {
            throw new LoadingException(suffixPath + filePath, $"fail to load xml file: {suffixPath + filePath}", ex);
        }
    }
    #endregion
}
