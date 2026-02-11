namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

[Table("customer_orders")]
public class CustomerOrder : BaseEntity
{
    public CustomerOrder()
        : base()
    {
    }

    public CustomerOrder(int id, string operationTime, int userId, int orderStateId)
        : base(id)
    {
        this.OperationTime = operationTime;
        this.UserId = userId;
        this.OrderStateId = orderStateId;
    }

    [Column("customer_id")]
    public int UserId { get; set; }

    [Column("operation_time")]
    public string OperationTime { get; set; } = null!; // schema stores as text

    [Column("order_state_id")]
    public int OrderStateId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(OrderStateId))]
    public OrderState State { get; set; } = null!;

    [InverseProperty(nameof(OrderDetail.Order))]
    public virtual IList<OrderDetail> Details { get; set; } = new List<OrderDetail>();
}
