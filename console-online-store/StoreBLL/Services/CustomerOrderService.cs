namespace StoreBLL.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using StoreBLL.Interfaces;
using StoreBLL.Models;
using StoreDAL.Data;
using StoreDAL.Repository;

/// <summary>
/// Provides CRUD operations and business logic for customer orders,
/// including order creation, cancellation, and delivery confirmation.
/// </summary>
public class CustomerOrderService : ICrud
{
    // Order state ids per TestDataFactory (consts first to satisfy SA1203)
    private const int StateNew = 1;
    private const int StateCanceledByUser = 2;
    private const int StateConfirmed = 4;
    private const int StateDelivered = 7;
    private const int StateConfirmedByClient = 8;

    private readonly CustomerOrderRepository orders;
    private readonly OrderDetailRepository details;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerOrderService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CustomerOrderService(StoreDbContext context)
    {
        this.orders = new CustomerOrderRepository(context);
        this.details = new OrderDetailRepository(context);
    }

    /// <summary>
    /// Adds a new customer order to the database.
    /// </summary>
    /// <param name="model">The order model to add.</param>
    public void Add(AbstractModel model)
    {
        var m = (CustomerOrderModel)model;
        var entity = new StoreDAL.Entities.CustomerOrder(
            0,
            string.IsNullOrWhiteSpace(m.OperationTime) ? DateTime.UtcNow.ToString("O") : m.OperationTime,
            m.UserId,
            m.StateId == 0 ? StateNew : m.StateId);

        this.orders.Add(entity);
        m.Id = entity.Id;
        m.StateId = entity.OrderStateId;
        m.StateName = "New Order";
    }

    /// <summary>
    /// Deletes an order by its identifier.
    /// </summary>
    /// <param name="modelId">The order identifier.</param>
    public void Delete(int modelId) => this.orders.DeleteById(modelId);

    /// <summary>
    /// Retrieves all orders.
    /// </summary>
    /// <returns>A collection of customer orders.</returns>
    public IEnumerable<AbstractModel> GetAll()
        => this.orders.GetAll().Select(this.Map);

    /// <summary>
    /// Retrieves all orders for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A list of customer orders belonging to the user.</returns>
    public IEnumerable<CustomerOrderModel> GetAllForUser(int userId)
        => this.orders.GetByUserId(userId).Select(this.Map).ToList();

    /// <summary>
    /// Retrieves a specific order by its identifier.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <returns>The customer order.</returns>
    public AbstractModel GetById(int id) => this.Map(this.orders.GetById(id));

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="model">The order model containing updated values.</param>
    public void Update(AbstractModel model)
    {
        var m = (CustomerOrderModel)model;
        var e = this.orders.GetById(m.Id);
        e.OperationTime = m.OperationTime;
        e.OrderStateId = m.StateId;
        this.orders.Update(e);
    }

    /// <summary>
    /// Creates a new order for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The created <see cref="CustomerOrderModel"/>.</returns>
    public CustomerOrderModel CreateOrder(int userId)
    {
        var m = new CustomerOrderModel(0, DateTime.UtcNow.ToString("O"), userId, StateNew, "New Order");
        this.Add(m);
        return m;
    }

    /// <summary>
    /// Cancels an order by the user if allowed (e.g., when new or confirmed).
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns><c>true</c> if cancellation succeeded, otherwise <c>false</c>.</returns>
    public bool CancelByUser(int orderId, int userId)
    {
        var e = this.orders.GetById(orderId);
        if (e.UserId != userId)
        {
            return false;
        }

        if (e.OrderStateId == StateNew || e.OrderStateId == StateConfirmed)
        {
            e.OrderStateId = StateCanceledByUser;
            this.orders.Update(e);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Confirms delivery of an order by the client.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns><c>true</c> if confirmation succeeded, otherwise <c>false</c>.</returns>
    public bool ConfirmDeliveryByClient(int orderId, int userId)
    {
        var e = this.orders.GetById(orderId);
        if (e.UserId != userId)
        {
            return false;
        }

        if (e.OrderStateId == StateDelivered)
        {
            e.OrderStateId = StateConfirmedByClient;
            this.orders.Update(e);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Maps a database entity <see cref="StoreDAL.Entities.CustomerOrder"/> to a <see cref="CustomerOrderModel"/>.
    /// </summary>
    /// <param name="o">The order entity.</param>
    /// <returns>The mapped order model.</returns>
    private CustomerOrderModel Map(StoreDAL.Entities.CustomerOrder o)
    {
        var m = new CustomerOrderModel(
            o.Id,
            o.OperationTime,
            o.UserId,
            o.OrderStateId,
            o.State?.StateName ?? string.Empty);

        var lines = this.details
            .GetByOrderId(o.Id)
            .Select(static d => new OrderDetailModel(
                d.Id,
                d.OrderId,
                d.ProductId,
                productTitle: d.Product.Title.Title,
                d.Product.Manufacturer.Name,
                d.Price,
                d.ProductAmount))
            .ToList();

        m.Details.AddRange(lines);
        return m;
    }
}
