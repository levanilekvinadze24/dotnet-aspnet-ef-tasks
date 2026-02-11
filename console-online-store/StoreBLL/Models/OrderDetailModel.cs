namespace StoreBLL.Models;

/// <summary>
/// Represents a single line item in a customer order.
/// </summary>
public class OrderDetailModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderDetailModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the order detail.</param>
    /// <param name="orderId">The identifier of the associated order.</param>
    /// <param name="productId">The identifier of the associated product.</param>
    /// <param name="productTitle">The title of the product.</param>
    /// <param name="manufacturer">The name of the product manufacturer.</param>
    /// <param name="price">The unit price of the product.</param>
    /// <param name="amount">The quantity of the product ordered.</param>
    public OrderDetailModel(int id, int orderId, int productId, string productTitle, string manufacturer, decimal price, int amount)
        : base(id)
    {
        this.OrderId = orderId;
        this.ProductId = productId;
        this.ProductTitle = productTitle;
        this.Manufacturer = manufacturer;
        this.Price = price;
        this.Amount = amount;
    }

    /// <summary>
    /// Gets or sets the identifier of the associated order.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    public string ProductTitle { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer of the product.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product ordered.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Gets the total cost of this order line (price × amount).
    /// </summary>
    public decimal LineTotal => this.Price * this.Amount;

    /// <summary>
    /// Returns a string representation of the order detail.
    /// </summary>
    /// <returns>A formatted string containing order detail info.</returns>
    public override string ToString() =>
        $"{this.Id}: [{this.OrderId}] {this.ProductTitle} x{this.Amount} @ {this.Price:C} = {this.LineTotal:C}";
}
