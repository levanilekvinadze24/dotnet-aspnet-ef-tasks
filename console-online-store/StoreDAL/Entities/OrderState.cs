namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

[Table("order_states")]
public class OrderState : BaseEntity
{
    public OrderState()
        : base()
    {
    }

    public OrderState(int id, string stateName)
        : base(id) => this.StateName = stateName;

    [Column("state_name")]
    public string StateName { get; set; } = null!;

    [InverseProperty(nameof(CustomerOrder.State))]
    public virtual IList<CustomerOrder> Order { get; set; } = new List<CustomerOrder>();
}
