namespace StoreDAL.Repository;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class ProductTitleRepository : AbstractRepository, IProductTitleRepository
{
    private readonly DbSet<ProductTitle> db;

    public ProductTitleRepository(StoreDbContext context)
        : base(context)
    {
        this.db = this.context.Set<ProductTitle>();
    }

    public void Add(ProductTitle entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(ProductTitle entity)
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

    public IEnumerable<ProductTitle> GetAll()
        => this.db.Include(t => t.Category).AsNoTracking().ToList();

    public IEnumerable<ProductTitle> GetAll(int pageNumber, int rowCount)
        => this.db.Include(t => t.Category).AsNoTracking()
             .Skip(System.Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public ProductTitle GetById(int id)
        => this.db.Include(t => t.Category).AsNoTracking().First(t => t.Id == id);

    public void Update(ProductTitle entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }
}
