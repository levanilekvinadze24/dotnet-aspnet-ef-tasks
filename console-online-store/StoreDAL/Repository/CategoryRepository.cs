namespace StoreDAL.Repository;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class CategoryRepository : AbstractRepository, ICategoryRepository
{
    private readonly DbSet<Category> db;

    public CategoryRepository(StoreDbContext context)
        : base(context)
    {
        this.db = this.context.Set<Category>();
    }

    public void Add(Category entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(Category entity)
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

    public IEnumerable<Category> GetAll()
        => this.db.Include(c => c.Titles).AsNoTracking().ToList();

    public IEnumerable<Category> GetAll(int pageNumber, int rowCount)
        => this.db.Include(c => c.Titles).AsNoTracking()
             .Skip(System.Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public Category GetById(int id)
        => this.db.Include(c => c.Titles).AsNoTracking().First(c => c.Id == id);

    public void Update(Category entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }
}
