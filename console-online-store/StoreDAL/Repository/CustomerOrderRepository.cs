namespace StoreDAL.Repository;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class CustomerOrderRepository : AbstractRepository, ICustomerOrderRepository
{
    private readonly DbSet<CustomerOrder> db;

    public CustomerOrderRepository(StoreDbContext context)
        : base(context)
    {
        this.db = this.context.Set<CustomerOrder>();
    }

    public void Add(CustomerOrder entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(CustomerOrder entity)
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

    public IEnumerable<CustomerOrder> GetAll()
        => this.db.Include(o => o.User)
             .Include(o => o.State)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Title)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .ToList();

    public IEnumerable<CustomerOrder> GetAll(int pageNumber, int rowCount)
        => this.db.Include(o => o.User)
             .Include(o => o.State)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Title)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .Skip(System.Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public CustomerOrder GetById(int id)
        => this.db.Include(o => o.User)
             .Include(o => o.State)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Title)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .First(o => o.Id == id);

    public void Update(CustomerOrder entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }

    // helper used by BLL
    public IEnumerable<CustomerOrder> GetByUserId(int userId)
        => this.db.Where(o => o.UserId == userId)
             .Include(o => o.State)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Title)
             .Include(o => o.Details).ThenInclude(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .ToList();
}
