using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

internal class Product : IProduct
{
    private readonly string productPath = @"products.xml";
    //private readonly string configPath = @"Config.xml";

    #region Product XElement  
    #region Converter/Casting
    public static IEnumerable<XElement> CreateProductElement(DO.Product productToCreate)
    {
        //id
        yield return new XElement("ID", productToCreate.ID);
        //title
        if (productToCreate.Name is not null)
            yield return new XElement("Name", productToCreate.Name);
        //artist
        if (productToCreate.Artist is not null)
            yield return new XElement("Artist", productToCreate.Artist);
        //style
        yield return new XElement("Style", productToCreate.Categories);
        //price
        yield return new XElement("Price", productToCreate.Price);
        //amount in stock
        yield return new XElement("AmountInStock", productToCreate.InStock);    
        //is deleted (?)
        yield return new XElement("IsDeleted", productToCreate.IsDeleted);
        if (productToCreate.Image is not null)
            yield return new XElement("Image", productToCreate.Image);

    }
    public static DO.Product? GetProduct(XElement p) =>
    p?.ToIntNullable("ID") is null ? null : new DO.Product()
    {
        ID = p.ToIntNullable("ID") ?? 0,
        Name = (string?)(p.Element("Name")?.Value),
        Artist = (string?)(p.Element("Artist")?.Value),
        Categories = StyleCast(p?.Element("Style")?.Value ?? "None"),
        InStock = p?.ToIntNullable("AmountInStock") ?? 0,   
        Price = p?.ToDoubleNullable("Price") ?? 0,
        IsDeleted = p?.ToBoolNullable("IsDeleted") ?? false,
        Image = (string?)p?.Element("Image")?.Value
    };

    private static DO.Enums.ArtStyles StyleCast(string style) => style switch
    {
        "Realism" => Enums.ArtStyles.Realism,
        "Cubism" => Enums.ArtStyles.Cubism,
        "SemiRealism" => Enums.ArtStyles.SemiRealism,
        "Abstract" => Enums.ArtStyles.Abstract,
        "Cartoon" => Enums.ArtStyles.Cartoon,
        "None" => Enums.ArtStyles.None,
        _ => Enums.ArtStyles.None,
    };

    //private XElement GetProductXelement(DO.Product p)
    //{
    //    return new XElement("Student", p.ID.ToString(), p.Name, p.Artist, p.Categories.ToString(), p.InStock.ToString(), p.IsDeleted.ToString(), p.Image);
    //}
    #endregion
    #region Data changing methods
    /// <summary>
    /// add new product to file
    /// </summary>
    /// <param name="productToCreate"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Add(DO.Product productToCreate)
    {
        XElement productsRootElem = XmlTool.LoadListFromXMLElement(productPath); //get all file

        //check if load works
        if (XmlTool.LoadListFromXMLElement(productPath)?.Elements()
            .FirstOrDefault(prod => prod.ToIntNullable("ID") == productToCreate.ID) is not null)
            throw new ObjectNotFoundException("id already exist");

        //check if the ID entered already exists in file
        if ((bool)(XmlTool.LoadListFromXMLElement(productPath)?.Elements()
            .Any(x => x.Element("ID")?.Value == productToCreate.ID.ToString()))!)
            throw new DoubleFoundException("found product with same ID - change ID");
        
        //add product to file
        productsRootElem.Add(new XElement("Student", CreateProductElement(productToCreate)));
        XmlTool.SaveListToXMLElement(productsRootElem, productPath);

        return productToCreate.ID;
        //XElement productRoot = XmlTool.LoadListFromXMLElement(productPath); //get all the elements from the file

        ////check if the product exists in the file
        //var customerFromFile = (from product in productRoot.Elements()
        //                        where (product?.Element("ID")?.Value == productToCreate.ID.ToString())
        //                        select product).FirstOrDefault();

        ////throw an exception
        //if (customerFromFile != null)
        //    throw new DoubleFoundException("the product already exists");

        ////add the product to the root element
        //productRoot.Add(
        //    new XElement("product",
        //    new XElement("ID", productToCreate.ID),
        //    new XElement("Title", productToCreate.Name),
        //    new XElement("Artist", productToCreate.Artist),
        //    new XElement("Price", productToCreate.Price),
        //    new XElement("AmountInStock", productToCreate.InStock),
        //    new XElement("Style",productToCreate.Categories),
        //    new XElement("IsDeleted", productToCreate.IsDeleted),
        //    new XElement("Image",productToCreate.Image)
        //    ));

        ////save the root in the file
        //XmlTool.SaveListToXMLElement(productRoot, productPath);
        //return productToCreate.ID;
    }

    /// <summary>
    /// delete existing product
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="ObjectNotFoundException"></exception>
    public void Delete(int id)
    {
        XElement productRootElem = XmlTool.LoadListFromXMLElement(productPath);

        (productRootElem.Elements()
            .FirstOrDefault(st => (int?)st.Element("ID") == id) ?? throw new ObjectNotFoundException("missing id"))
            .Element("IsDeleted")!.Value = "true";

        XmlTool.SaveListToXMLElement(productRootElem, productPath);
        //XElement productRoot = XmlTool.LoadListFromXMLElement(productPath); //get all the elements from the file
        //var productElem = (from product in productRoot.Elements()
        //                      where product.Element("ID")?.Value == id.ToString()
        //                      select product).FirstOrDefault();
        //if (productElem == null)
        //    throw new InfoMissException(id, "missing information");

        //productElem.Remove();
        //XmlTool.SaveListToXMLElement(productRoot, productPath);
    }    
    
    /// <summary>
    /// update an existing product
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Product item)
    {
        //Delete(item.ID);
        //Add(item);

        XElement productRoot = XmlTool.LoadListFromXMLElement(productPath); //get all the elements from the file

        //check if the product exists in the file
        var productFromFile = (from product in productRoot.Elements()
                               where (product?.Element("ID")?.Value == item.ID.ToString())
                               select product).FirstOrDefault();
        //throw an exception
        if (productFromFile == null)
            throw new ObjectNotFoundException("the product doesn't exist");

        DO.Product productToUpdate = GetById(item.ID);
        //save updated to file
        if (productToUpdate.Name != item.Name)
            productFromFile.Element("Title")!.Value = item.Name ?? "";
        if (productToUpdate.Artist != item.Artist)
            productFromFile.Element("Artist")!.Value = item.Artist ?? "";
        if (productToUpdate.Image != item.Image)
            productFromFile.Element("Image")!.Value = item.Image ?? "";
        if (productToUpdate.Price != item.Price)
            productFromFile.Element("Price")!.Value = item.Price.ToString();
        if (productToUpdate.InStock != item.InStock)
            productFromFile.Element("AmountInStock")!.Value = item.InStock.ToString();
        if (productToUpdate.Categories != item.Categories)
            productFromFile.Element("Style")!.Value = item.Categories.ToString();
        if (productToUpdate.IsDeleted != item.IsDeleted)
            productFromFile.Element("IsDeleted")!.Value = item.IsDeleted.ToString();


        XmlTool.SaveListToXMLElement(productRoot, productPath);
    }
    #endregion
    #region Get methods
    /// <summary>
    /// get a collection of products based on "filter" constraints
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<DO.Product?> GetAll(Func<DO.Product?, bool>? filter = null)
        =>
        filter is null
        ? XmlTool.LoadListFromXMLElement(productPath).Elements().Select(p => GetProduct(p))
        : XmlTool.LoadListFromXMLElement(productPath).Elements().Select(p => GetProduct(p)).Where(filter);
    //{
    //    XElement productRoot = XmlTool.LoadListFromXMLElement(productPath); //get all the elements from the file

    //    return (IEnumerable<DO.Product?>)(from product in productRoot.Elements()
    //                                      let productToCheck = new DO.Product()
    //                                      {
    //                                          ID = int.Parse(product.Element("ID")!.Value),
    //                                          Name = product.Element("Title")?.Value,
    //                                          Artist = product.Element("Artist")?.Value,
    //                                          Price = double.Parse(product.Element("Price")!.Value),
    //                                          InStock = int.Parse(product.Element("AmountInStock")!.Value),
    //                                          Categories = StyleCast(product.Element("Style")!.Value),
    //                                          IsDeleted = bool.Parse(product.Element("IsDeleted")!.Value),
    //                                          Image = product?.Element("Image")?.Value
    //                                      }
    //                                      where (filter(productToCheck) == null) ?? null
    //                                      select productToCheck).ToList();
    //                                      //select productToCheck).ToList();
    //}

    public DO.Product GetByFilter(Func<DO.Product?, bool> filter) =>
        (DO.Product)(filter is null
        ? XmlTool.LoadListFromXMLElement(productPath).Elements().Select(p => GetProduct(p))
        : XmlTool.LoadListFromXMLElement(productPath).Elements().Select(p => GetProduct(p)).Where(filter)).FirstOrDefault()!;
    //{
    //    throw new NotImplementedException();
    //}

    /// <summary>
    /// get a specific product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public DO.Product GetById(int id) =>
        (DO.Product)GetProduct(XmlTool.LoadListFromXMLElement(productPath)?.Elements()
        .FirstOrDefault(st => st.ToIntNullable("ID") == id)
        // fix to: throw new DalMissingIdException(id);
        ?? throw new Exception("missing id"))!;
    //{
    //    XElement productRoot = XmlTool.LoadListFromXMLElement(productPath); //get all the elements from the file

    //    //check if the product exists in th file
    //    var productFromFile = (from product in productRoot.Elements()
    //                            where product.Element("ID")?.Value == id.ToString()
    //                            select product).FirstOrDefault();

    //    //throw an exception
    //    if (productFromFile == null)
    //        throw new ObjectNotFoundException("the product doesn't exist");

    //    return new DO.Product()
    //    {
    //        ID = id,
    //        Name = productFromFile.Element("Title")?.Value,
    //        Artist = productFromFile.Element("Artist")?.Value,
    //        Price = double.Parse(productFromFile.Element("Price")!.Value),
    //        InStock = int.Parse(productFromFile.Element("AmountInStock")!.Value),
    //        Categories = StyleCast(productFromFile.Element("Style")!.Value),
    //        IsDeleted = bool.Parse(productFromFile.Element("IsDeleted")!.Value),
    //        Image = productFromFile?.Element("Image")?.Value
    //    };
    //}
    #endregion
    #endregion
}
