namespace StoreDAL.Repository;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class ManufacturerRepository : AbstractRepository, IManufacturerRepository
{
    private readonly DbSet<Manufacturer> db;

    public ManufacturerRepository(StoreDbContext context)
        : base(context)
    {
        this.db = this.context.Set<Manufacturer>();
    }

    public void Add(Manufacturer entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(Manufacturer entity)
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

    public IEnumerable<Manufacturer> GetAll()
        => this.db.AsNoTracking().ToList();

    public IEnumerable<Manufacturer> GetAll(int pageNumber, int rowCount)
        => this.db.AsNoTracking()
             .Skip(System.Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public Manufacturer GetById(int id)
        => this.db.AsNoTracking().First(m => m.Id == id);

    public void Update(Manufacturer entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }
}
