using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Product
{
    /// <summary>
    /// ID number of product
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Name of product
    /// </summary>
    public string? NameOfProduct { get; set; }
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
    public int AmountInStock { get; set; }
    /// <summary>
    /// Image of a product
    /// </summary>
    public string? Image { get; set; }
    /// <summary>
    /// print Product
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return this.ToStringProperty();
    }
}