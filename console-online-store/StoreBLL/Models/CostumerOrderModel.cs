namespace StoreBLL.Models;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a customer order, including order metadata and its details.
/// </summary>
public class CustomerOrderModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerOrderModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="operationTime">The time when the order was created or updated.</param>
    /// <param name="userId">The identifier of the user who placed the order.</param>
    /// <param name="stateId">The identifier of the order state.</param>
    /// <param name="stateName">The descriptive name of the order state.</param>
    public CustomerOrderModel(int id, string operationTime, int userId, int stateId, string stateName)
        : base(id)
    {
        this.OperationTime = operationTime;
        this.UserId = userId;
        this.StateId = stateId;
        this.StateName = stateName;
    }

    /// <summary>
    /// Gets or sets the operation timestamp of the order (ISO 8601 format).
    /// </summary>
    public string OperationTime { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who placed the order.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the current state of the order.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Gets or sets the descriptive name of the current order state.
    /// </summary>
    public string StateName { get; set; }

    /// <summary>
    /// Gets or sets the list of order details (individual line items).
    /// </summary>
    public List<OrderDetailModel> Details { get; set; } = new List<OrderDetailModel>();

    /// <summary>
    /// Gets the total cost of the order (sum of all line totals).
    /// </summary>
    public decimal Total => this.Details.Sum(d => d.LineTotal);

    /// <summary>
    /// Returns a string representation of the order, including its Id, state, time, and total.
    /// </summary>
    /// <returns>A formatted string describing the order.</returns>
    public override string ToString() =>
        $"Order #{this.Id} | {this.StateName} | {this.OperationTime} | Total: {this.Total:C}";
}
