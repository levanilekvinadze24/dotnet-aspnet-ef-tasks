namespace StoreBLL.Models;

/// <summary>
/// Represents a manufacturer that produces products in the store.
/// </summary>
public class ManufacturerModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ManufacturerModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the manufacturer.</param>
    /// <param name="name">The name of the manufacturer.</param>
    public ManufacturerModel(int id, string name)
        : base(id)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets or sets the name of the manufacturer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Returns a string representation of the manufacturer, including Id and Name.
    /// </summary>
    /// <returns>A formatted string with Id and Name.</returns>
    public override string ToString() => $"Id:{this.Id} {this.Name}";
}
