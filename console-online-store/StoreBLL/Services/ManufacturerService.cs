namespace StoreBLL.Services;

using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations for <see cref="ManufacturerModel"/> using the repository pattern.
/// </summary>
public class ManufacturerService : ICrud
{
    private readonly ManufacturerRepository repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManufacturerService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ManufacturerService(StoreDbContext context)
    {
        this.repo = new ManufacturerRepository(context);
    }

    /// <summary>
    /// Adds a new manufacturer to the database.
    /// </summary>
    /// <param name="model">The manufacturer model to add.</param>
    public void Add(AbstractModel model)
    {
        var m = (ManufacturerModel)model;
        var e = new Manufacturer(0, m.Name);
        this.repo.Add(e);
        m.Id = e.Id;
    }

    /// <summary>
    /// Deletes a manufacturer by its identifier.
    /// </summary>
    /// <param name="modelId">The manufacturer identifier.</param>
    public void Delete(int modelId) => this.repo.DeleteById(modelId);

    /// <summary>
    /// Retrieves all manufacturers from the database.
    /// </summary>
    /// <returns>A collection of <see cref="AbstractModel"/> representing manufacturers.</returns>
    public IEnumerable<AbstractModel> GetAll()
        => this.repo.GetAll().Select(x => new ManufacturerModel(x.Id, x.Name));

    /// <summary>
    /// Retrieves a manufacturer by its identifier.
    /// </summary>
    /// <param name="id">The manufacturer identifier.</param>
    /// <returns>A <see cref="ManufacturerModel"/> instance.</returns>
    public AbstractModel GetById(int id)
    {
        var e = this.repo.GetById(id);
        return new ManufacturerModel(e.Id, e.Name);
    }

    /// <summary>
    /// Updates an existing manufacturer in the database.
    /// </summary>
    /// <param name="model">The manufacturer model with updated data.</param>
    public void Update(AbstractModel model)
    {
        var m = (ManufacturerModel)model;
        var e = this.repo.GetById(m.Id);
        e.Name = m.Name;
        this.repo.Update(e);
    }
}
