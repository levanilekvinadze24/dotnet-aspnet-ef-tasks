namespace StoreBLL.Models;

/// <summary>
/// Represents a product title (a named product type) within a category.
/// </summary>
public class ProductTitleModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductTitleModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the product title.</param>
    /// <param name="title">The display title of the product.</param>
    /// <param name="categoryId">The identifier of the category to which the product belongs.</param>
    /// <param name="categoryName">The name of the category.</param>
    public ProductTitleModel(int id, string title, int categoryId, string categoryName)
        : base(id)
    {
        this.Title = title;
        this.CategoryId = categoryId;
        this.CategoryName = categoryName;
    }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the category to which the product belongs.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the name of the category to which the product belongs.
    /// </summary>
    public string CategoryName { get; set; }

    /// <summary>
    /// Returns a string representation of the product title including Id, Title, and Category.
    /// </summary>
    /// <returns>A formatted string with product title details.</returns>
    public override string ToString() => $"Id:{this.Id} {this.Title} [{this.CategoryName}]";
}
