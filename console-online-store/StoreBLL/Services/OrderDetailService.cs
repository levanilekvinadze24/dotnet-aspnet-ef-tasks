namespace StoreBLL.Services;

using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Repository;

/// <summary>
/// Provides operations for managing order details (lines in a customer order).
/// </summary>
public class OrderDetailService : ICrud
{
    private readonly OrderDetailRepository details;
    private readonly ProductRepository products;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderDetailService"/> class.
    /// </summary>
    /// <param name="context">Database context.</param>
    public OrderDetailService(StoreDbContext context)
    {
        this.details = new OrderDetailRepository(context);
        this.products = new ProductRepository(context);
    }

    /// <inheritdoc/>
    public void Add(AbstractModel model)
    {
        var m = (OrderDetailModel)model;
        var product = this.products.GetById(m.ProductId);
        var price = product.UnitPrice;

        var entity = new StoreDAL.Entities.OrderDetail(
            0,
            m.OrderId,
            m.ProductId,
            price,
            m.Amount);

        this.details.Add(entity);

        // fill back
        m.Id = entity.Id;
        m.Price = price;
        m.ProductTitle = product.Title.Title;
        m.Manufacturer = product.Manufacturer.Name;
    }

    /// <inheritdoc/>
    public void Delete(int modelId) => this.details.DeleteById(modelId);

    /// <inheritdoc/>
    public IEnumerable<AbstractModel> GetAll()
        => this.details.GetAll().Select(d => new OrderDetailModel(
            d.Id,
            d.OrderId,
            d.ProductId,
            productTitle: d.Product.Title.Title,
            d.Product.Manufacturer.Name,
            d.Price,
            d.ProductAmount));

    /// <summary>
    /// Gets all order details for a specific order.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <returns>List of order detail models.</returns>
    public IEnumerable<OrderDetailModel> GetForOrder(int orderId)
        => this.details.GetByOrderId(orderId).Select(d => new OrderDetailModel(
            d.Id,
            d.OrderId,
            d.ProductId,
            d.Product.Title.Title,
            d.Product.Manufacturer.Name,
            d.Price,
            d.ProductAmount)).ToList();

    /// <inheritdoc/>
    public AbstractModel GetById(int id)
    {
        var d = this.details.GetById(id);
        return new OrderDetailModel(
            d.Id,
            d.OrderId,
            d.ProductId,
            d.Product.Title.Title,
            d.Product.Manufacturer.Name,
            d.Price,
            d.ProductAmount);
    }

    /// <inheritdoc/>
    public void Update(AbstractModel model)
    {
        var m = (OrderDetailModel)model;
        var e = this.details.GetById(m.Id);
        e.ProductAmount = m.Amount;
        e.Price = m.Price; // keep captured price unless you want to recalc
        this.details.Update(e);
    }
}
