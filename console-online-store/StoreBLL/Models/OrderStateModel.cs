namespace StoreBLL.Models;

/// <summary>
/// Represents the state of a customer order (e.g., New, Confirmed, Delivered).
/// </summary>
public class OrderStateModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStateModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the order state.</param>
    /// <param name="stateName">The descriptive name of the order state.</param>
    public OrderStateModel(int id, string stateName)
        : base(id)
    {
        this.Id = id;
        this.StateName = stateName;
    }

    /// <summary>
    /// Gets or sets the descriptive name of the order state.
    /// </summary>
    public string StateName { get; set; }

    /// <summary>
    /// Returns a string representation of the order state, including Id and Name.
    /// </summary>
    /// <returns>A formatted string containing the order state information.</returns>
    public override string ToString()
    {
        return $"Id:{this.Id} {this.StateName}";
    }
}
