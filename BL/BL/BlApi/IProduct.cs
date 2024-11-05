using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BO;
namespace BlApi;
/// <summary>
/// Operations on Products, for both manager and catalog (customer)
/// </summary>
public interface IProduct
{
    /// <summary>
    /// Get list of products - for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ProductForList?> GetProductList();
    /// <summary>
    /// Get list of products - for catalog
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.ProductItem?> GetProductItemList();
    /// <summary>
    /// Manager - Get product 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Product? GetProductByIdForManager(int id);
    /// <summary>
    /// Catalog - get product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ProductItem? GetProductByIdForCustomer(int id, Cart? c);
    /// <summary>
    /// Get list of products grouped by category - for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<BO.Enums.ArtStyles, BO.ProductForList?>> GetAll_GroupedByCategory_Manager();
    /// <summary>
    /// Get list of products grouped by artist name - for manager
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<string?, BO.ProductForList?>> GetAll_GroupedByArtistName_Manager();
    /// <summary>
    /// Get list of products grouped by category - for customer catalog
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<BO.Enums.ArtStyles, BO.ProductItem?>> GetAll_GroupedByCategory_Customer();
    /// <summary>
    /// Get list of products grouped by artist name - for customer catalog
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IGrouping<string?, BO.ProductItem?>> GetAll_GroupedByArtistName_Customer();
    /// <summary>
    /// Add product to list - for manager
    /// </summary>
    /// <param name="prod"></param>
    public void Add(Product prod);
    /// <summary>
    /// Delete from list - for manager
    /// </summary>
    /// <param name="idProd"></param>
    public void Delete(int idProd);
    /// <summary>
    /// Update list - for manager
    /// </summary>
    /// <param name="prod"></param>
    public void Update(Product prod);
}
