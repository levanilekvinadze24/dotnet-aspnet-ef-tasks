namespace StoreDAL.Repository;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreDAL.Data;
using StoreDAL.Entities;
using StoreDAL.Interfaces;

public class OrderDetailRepository : AbstractRepository, IOrderDetailRepository
{
    private readonly DbSet<OrderDetail> db;

    public OrderDetailRepository(StoreDbContext context)
        : base(context)
    {
        this.db = this.context.Set<OrderDetail>();
    }

    public void Add(OrderDetail entity)
    {
        this.db.Add(entity);
        this.context.SaveChanges();
    }

    public void Delete(OrderDetail entity)
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

    public IEnumerable<OrderDetail> GetAll()
        => this.db.Include(d => d.Order)
             .Include(d => d.Product).ThenInclude(p => p.Title)
             .Include(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .ToList();

    public IEnumerable<OrderDetail> GetAll(int pageNumber, int rowCount)
        => this.db.Include(d => d.Order)
             .Include(d => d.Product).ThenInclude(p => p.Title)
             .Include(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .Skip(System.Math.Max(0, (pageNumber - 1) * rowCount))
             .Take(rowCount)
             .ToList();

    public OrderDetail GetById(int id)
        => this.db.Include(d => d.Order)
             .Include(d => d.Product).ThenInclude(p => p.Title)
             .Include(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .First(d => d.Id == id);

    public void Update(OrderDetail entity)
    {
        this.db.Update(entity);
        this.context.SaveChanges();
    }

    // helper used by BLL
    public IEnumerable<OrderDetail> GetByOrderId(int orderId)
        => this.db.Where(d => d.OrderId == orderId)
             .Include(d => d.Product).ThenInclude(p => p.Title)
             .Include(d => d.Product).ThenInclude(p => p.Manufacturer)
             .AsNoTracking()
             .ToList();
}
