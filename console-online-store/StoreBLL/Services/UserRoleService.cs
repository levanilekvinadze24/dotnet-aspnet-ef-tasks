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
/// Provides CRUD operations for <see cref="UserRoleModel"/>.
/// </summary>
public class UserRoleService : ICrud
{
    private readonly UserRoleRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRoleService(StoreDbContext context)
    {
        this.repository = new UserRoleRepository(context);
    }

    /// <summary>
    /// Adds a new user role to the database.
    /// </summary>
    /// <param name="model">The user role model to add.</param>
    public void Add(AbstractModel model)
    {
        var x = (UserRoleModel)model;
        this.repository.Add(new UserRole(x.Id, x.RoleName));
    }

    /// <summary>
    /// Deletes a user role by its identifier.
    /// </summary>
    /// <param name="modelId">The role identifier.</param>
    public void Delete(int modelId)
    {
        this.repository.DeleteById(modelId);
    }

    /// <summary>
    /// Retrieves all user roles.
    /// </summary>
    /// <returns>A collection of <see cref="UserRoleModel"/>.</returns>
    public IEnumerable<AbstractModel> GetAll()
    {
        return this.repository.GetAll()
            .Select(x => new UserRoleModel(x.Id, x.RoleName));
    }

    /// <summary>
    /// Retrieves a user role by its identifier.
    /// </summary>
    /// <param name="id">The role identifier.</param>
    /// <returns>A <see cref="UserRoleModel"/> instance.</returns>
    public AbstractModel GetById(int id)
    {
        var res = this.repository.GetById(id);
        return new UserRoleModel(res.Id, res.RoleName);
    }

    /// <summary>
    /// Updates an existing user role.
    /// (Currently not implemented.)
    /// </summary>
    /// <param name="model">The updated role model.</param>
    public void Update(AbstractModel model)
    {
        throw new NotImplementedException();
    }
}
