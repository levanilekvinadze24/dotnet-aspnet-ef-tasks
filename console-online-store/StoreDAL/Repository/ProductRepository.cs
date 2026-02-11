namespace StoreDAL.Repository;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class ProductRepository : AbstractRepository, IProductRepository
{
    private readonly DbSet<Product> db;

    public ProductRepository(StoreDbContext context)
        : base(context)
    {
        this.db = this.context.Set<Product>();
    }

    public void Add(Product entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(Product entity)
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

    public IEnumerable<Product> GetAll()
        => this.db.Include(p => p.Title).ThenInclude(t => t.Category)
             .Include(p => p.Manufacturer)
             .AsNoTracking()
             .ToList();

    public IEnumerable<Product> GetAll(int pageNumber, int rowCount)
        => this.db.Include(p => p.Title).ThenInclude(t => t.Category)
             .Include(p => p.Manufacturer)
             .AsNoTracking()
             .Skip(System.Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public Product GetById(int id)
        => this.db.Include(p => p.Title).ThenInclude(t => t.Category)
             .Include(p => p.Manufacturer)
             .AsNoTracking()
             .First(p => p.Id == id);

    public void Update(Product entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }
}
