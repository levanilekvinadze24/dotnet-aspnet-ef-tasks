namespace StoreBLL.Services;

using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations for <see cref="ProductModel"/> using the repository pattern.
/// </summary>
public class ProductService : ICrud
{
    private readonly ProductRepository products;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductService(StoreDbContext context)
    {
        this.products = new ProductRepository(context);
    }

    /// <summary>
    /// Adds a new product to the database.
    /// (Currently not implemented.)
    /// </summary>
    /// <param name="model">The product model to add.</param>
    public void Add(AbstractModel model) => throw new System.NotImplementedException();

    /// <summary>
    /// Deletes a product by its identifier.
    /// (Currently not implemented.)
    /// </summary>
    /// <param name="modelId">The product identifier.</param>
    public void Delete(int modelId) => throw new System.NotImplementedException();

    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>A collection of <see cref="ProductModel"/>.</returns>
    public IEnumerable<AbstractModel> GetAll()
        => this.products.GetAll()
                   .Select(p => new ProductModel(
                       p.Id,
                       p.Title.Title,
                       p.Title.Category.Name,
                       p.Manufacturer.Name,
                       p.UnitPrice,
                       p.Description ?? string.Empty));

    /// <summary>
    /// Retrieves a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A <see cref="ProductModel"/> instance.</returns>
    public AbstractModel GetById(int id)
    {
        var p = this.products.GetById(id);
        return new ProductModel(
            p.Id,
            p.Title.Title,
            p.Title.Category.Name,
            p.Manufacturer.Name,
            p.UnitPrice,
            p.Description ?? string.Empty);
    }

    /// <summary>
    /// Updates an existing product.
    /// (Currently not implemented.)
    /// </summary>
    /// <param name="model">The updated product model.</param>
    public void Update(AbstractModel model) => throw new System.NotImplementedException();
}
