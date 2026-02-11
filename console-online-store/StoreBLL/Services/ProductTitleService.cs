namespace StoreBLL.Services;

using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations for <see cref="ProductTitleModel"/> using the repository pattern.
/// </summary>
public class ProductTitleService : ICrud
{
    private readonly ProductTitleRepository repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductTitleService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductTitleService(StoreDbContext context)
    {
        this.repo = new ProductTitleRepository(context);
    }

    /// <summary>
    /// Adds a new product title to the database.
    /// </summary>
    /// <param name="model">The product title model to add.</param>
    public void Add(AbstractModel model)
    {
        var m = (ProductTitleModel)model;
        var e = new ProductTitle(0, m.Title, m.CategoryId);
        this.repo.Add(e);
        m.Id = e.Id;
    }

    /// <summary>
    /// Deletes a product title by its identifier.
    /// </summary>
    /// <param name="modelId">The product title identifier.</param>
    public void Delete(int modelId) => this.repo.DeleteById(modelId);

    /// <summary>
    /// Retrieves all product titles from the database.
    /// </summary>
    /// <returns>A collection of <see cref="ProductTitleModel"/>.</returns>
    public IEnumerable<AbstractModel> GetAll()
        => this.repo.GetAll().Select(t => new ProductTitleModel(t.Id, t.Title, t.CategoryId, t.Category.Name));

    /// <summary>
    /// Retrieves a product title by its identifier.
    /// </summary>
    /// <param name="id">The product title identifier.</param>
    /// <returns>A <see cref="ProductTitleModel"/> instance.</returns>
    public AbstractModel GetById(int id)
    {
        var t = this.repo.GetById(id);
        return new ProductTitleModel(t.Id, t.Title, t.CategoryId, t.Category?.Name ?? string.Empty);
    }

    /// <summary>
    /// Updates an existing product title in the database.
    /// </summary>
    /// <param name="model">The product title model containing updated values.</param>
    public void Update(AbstractModel model)
    {
        var m = (ProductTitleModel)model;
        var e = this.repo.GetById(m.Id);
        e.Title = m.Title;
        e.CategoryId = m.CategoryId;
        this.repo.Update(e);
    }
}
