namespace StoreBLL.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreBLL.Security;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations and auth helpers for <see cref="UserModel"/>.
/// </summary>
public class UserService : ICrud
{
    private readonly UserRepository users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserService(StoreDbContext context)
    {
        this.users = new UserRepository(context);
    }

    /// <summary>
    /// Registers a new user (Guest flow). Expects <see cref="UserModel.Password"/> to be set.
    /// </summary>
    /// <param name="model">The user model to add.</param>
    /// <exception cref="InvalidOperationException">Thrown when the login already exists.</exception>
    public void Add(AbstractModel model)
    {
        var m = (UserModel)model;

        // simple duplicate check
        if (this.users.GetAll().Any(x => x.Login == m.Login))
        {
            throw new InvalidOperationException("Login already exists.");
        }

        var hash = PasswordHasher.Hash(m.Password ?? string.Empty);
        var entity = new User(
            0,
            m.FirstName,
            m.LastName,
            m.Login,
            hash,
            m.RoleId == 0 ? 2 : m.RoleId); // 2 = Registered

        this.users.Add(entity);
        m.Id = entity.Id;
    }

    /// <summary>
    /// Deletes a user by identifier.
    /// </summary>
    /// <param name="modelId">The user identifier.</param>
    public void Delete(int modelId) => this.users.DeleteById(modelId);

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A sequence of <see cref="UserModel"/>.</returns>
    public IEnumerable<AbstractModel> GetAll()
        => this.users.GetAll()
            .Select(u => new UserModel(
                u.Id,
                u.Name,
                u.LastName,
                u.Login,
                u.RoleId,
                u.Role?.RoleName ?? string.Empty));

    /// <summary>
    /// Retrieves a user by identifier.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns>The requested <see cref="UserModel"/>.</returns>
    public AbstractModel GetById(int id)
    {
        var u = this.users.GetById(id);
        return new UserModel(
            u.Id,
            u.Name,
            u.LastName,
            u.Login,
            u.RoleId,
            u.Role?.RoleName ?? string.Empty);
    }

    /// <summary>
    /// Updates an existing user. If <see cref="UserModel.Password"/> is provided, it is re-hashed and saved.
    /// </summary>
    /// <param name="model">The updated user model.</param>
    public void Update(AbstractModel model)
    {
        var m = (UserModel)model;
        var u = this.users.GetById(m.Id);
        u.Name = m.FirstName;
        u.LastName = m.LastName;
        u.Login = m.Login;

        if (!string.IsNullOrEmpty(m.Password))
        {
            u.Password = PasswordHasher.Hash(m.Password);
        }

        if (m.RoleId != 0)
        {
            u.RoleId = m.RoleId;
        }

        this.users.Update(u);
    }

    /// <summary>
    /// Authenticates a user by login and plain-text password.
    /// </summary>
    /// <param name="login">The login name.</param>
    /// <param name="plainPassword">The plain-text password.</param>
    /// <returns>
    /// A populated <see cref="UserModel"/> when credentials are valid; otherwise <see langword="null"/>.
    /// </returns>
    public UserModel? Authenticate(string login, string plainPassword)
    {
        var u = this.users.GetByLogin(login);
        if (u is null)
        {
            return null;
        }

        if (!string.Equals(u.Password, PasswordHasher.Hash(plainPassword), StringComparison.Ordinal))
        {
            return null;
        }

        return new UserModel(
            u.Id,
            u.Name,
            u.LastName,
            u.Login,
            u.RoleId,
            u.Role?.RoleName ?? string.Empty);
    }
}
