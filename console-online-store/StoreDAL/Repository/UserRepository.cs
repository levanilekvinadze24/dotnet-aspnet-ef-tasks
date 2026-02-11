namespace StoreDAL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class UserRepository : AbstractRepository, IUserRepository
{
    private readonly DbSet<User> db;

    public UserRepository(StoreDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.db = context.Set<User>();
    }

    public void Add(User entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(User entity)
    {
        this.db.Remove(entity);
        this.context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var e = this.db.Find(id);
        if (e != null)
        {
            this.db.Remove(e);
            this.context.SaveChanges();
        }
    }

    public IEnumerable<User> GetAll()
        => this.db.Include(x => x.Role).AsNoTracking().ToList();

    public IEnumerable<User> GetAll(int pageNumber, int rowCount)
        => this.db.Include(x => x.Role).AsNoTracking()
             .Skip(Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public User GetById(int id)
        => this.db.Include(x => x.Role).AsNoTracking().First(x => x.Id == id);

    public void Update(User entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }

    // helper for login (extra; not on interface)
    public User? GetByLogin(string login)
        => this.db.Include(u => u.Role).FirstOrDefault(u => u.Login == login);
}
