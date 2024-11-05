using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;
/// <summary>
/// Structure for all products
/// </summary>
public struct Product
{
    /// <summary>
    /// ID number of product
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Name of product/ art piece
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Name of artist
    /// </summary>
    public string? Artist { get; set; }
    /// <summary>
    /// Category of items
    /// </summary>
    public Enums.ArtStyles Categories { get; set; }
    /// <summary>
    /// Price of product
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// Amount of product in stock
    /// </summary>
    public int InStock { get; set; }  
    /// <summary>
    /// Image of a product
    /// </summary>
    public string? Image { get; set; }
    /// <summary>
    /// Indicates whether the order has been deleted
    /// </summary>
    public bool IsDeleted { get; set; }
    /// <summary>
    /// Prints details of a product
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}