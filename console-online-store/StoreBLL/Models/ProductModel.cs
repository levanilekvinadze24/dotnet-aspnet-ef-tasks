namespace StoreBLL.Models;

/// <summary>
/// Represents a product in the store, including its category, manufacturer, and details.
/// </summary>
public class ProductModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="title">The product title.</param>
    /// <param name="category">The product category name.</param>
    /// <param name="manufacturer">The name of the manufacturer.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <param name="description">The description of the product.</param>
    public ProductModel(int id, string title, string category, string manufacturer, decimal unitPrice, string description)
        : base(id)
    {
        this.Title = title;
        this.Category = category;
        this.Manufacturer = manufacturer;
        this.UnitPrice = unitPrice;
        this.Description = description;
    }

    /// <summary>
    /// Gets or sets the product title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the category name of the product.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the name of the manufacturer.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Returns a string representation of the product.
    /// </summary>
    /// <returns>A formatted string containing product details.</returns>
    public override string ToString() =>
        $"{this.Id}. {this.Title} | {this.Manufacturer} | {this.Category} | {this.UnitPrice:C} — {this.Description}";
}
