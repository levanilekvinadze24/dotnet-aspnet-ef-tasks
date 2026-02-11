namespace StoreBLL.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations for <see cref="OrderStateModel"/> using the repository pattern.
/// </summary>
public class OrderStateService : ICrud
{
    private readonly OrderStateRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderStateService(StoreDbContext context)
    {
        this.repository = new OrderStateRepository(context);
    }

    /// <summary>
    /// Adds a new order state to the database.
    /// </summary>
    /// <param name="model">The order state model to add.</param>
    public void Add(AbstractModel model)
    {
        var x = (OrderStateModel)model;
        this.repository.Add(new OrderState(x.Id, x.StateName));
    }

    /// <summary>
    /// Deletes an order state by its identifier.
    /// </summary>
    /// <param name="modelId">The order state identifier.</param>
    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    /// <summary>
    /// Retrieves all order states.
    /// </summary>
    /// <returns>A collection of <see cref="OrderStateModel"/>.</returns>
    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll().Select(x => new OrderStateModel(x.Id, x.StateName));
    }

    /// <summary>
    /// Retrieves an order state by its identifier.
    /// </summary>
    /// <param name="id">The order state identifier.</param>
    /// <returns>An <see cref="OrderStateModel"/> instance.</returns>
    public AbstractModel GetById(int id)
    {
        var res = this.repository.GetById(id);
        return new OrderStateModel(res.Id, res.StateName);
    }

    /// <summary>
    /// Updates an existing order state in the database.
    /// (Currently not implemented.)
    /// </summary>
    /// <param name="model">The updated order state model.</param>
    public void Update(AbstractModel model)
    {
        throw new NotImplementedException();
    }
}
