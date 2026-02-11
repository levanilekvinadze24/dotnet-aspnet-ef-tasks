namespace StoreBLL.Models;

/// <summary>
/// Represents a category in the store (e.g., Fruits, Electronics).
/// </summary>
public class CategoryModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryModel"/> class.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="name">The category name.</param>
    public CategoryModel(int id, string name)
        : base(id)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Returns a string representation of the category.
    /// </summary>
    /// <returns>A formatted string containing the Id and Name.</returns>
    public override string ToString() => $"Id:{this.Id} {this.Name}";
}
