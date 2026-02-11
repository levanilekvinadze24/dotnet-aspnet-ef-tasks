namespace StoreBLL.Services;

using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations for <see cref="CategoryModel"/> using the repository pattern.
/// </summary>
public class CategoryService : ICrud
{
    private readonly CategoryRepository repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CategoryService(StoreDbContext context)
    {
        this.repo = new CategoryRepository(context);
    }

    /// <summary>
    /// Adds a new category to the database.
    /// </summary>
    /// <param name="model">The category model to add.</param>
    public void Add(AbstractModel model)
    {
        var m = (CategoryModel)model;
        var e = new Category(0, m.Name);
        this.repo.Add(e);
        m.Id = e.Id;
    }

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="modelId">The category identifier.</param>
    public void Delete(int modelId) => this.repo.DeleteById(modelId);

    /// <summary>
    /// Retrieves all categories from the database.
    /// </summary>
    /// <returns>A collection of <see cref="AbstractModel"/> representing categories.</returns>
    public IEnumerable<AbstractModel> GetAll()
        => this.repo.GetAll().Select(c => new CategoryModel(c.Id, c.Name));

    /// <summary>
    /// Retrieves a category by its identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>A <see cref="CategoryModel"/> instance.</returns>
    public AbstractModel GetById(int id)
    {
        var c = this.repo.GetById(id);
        return new CategoryModel(c.Id, c.Name);
    }

    /// <summary>
    /// Updates an existing category in the database.
    /// </summary>
    /// <param name="model">The category model with updated data.</param>
    public void Update(AbstractModel model)
    {
        var m = (CategoryModel)model;
        var e = this.repo.GetById(m.Id);
        e.Name = m.Name;
        this.repo.Update(e);
    }
}
