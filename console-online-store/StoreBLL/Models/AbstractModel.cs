namespace StoreBLL.Models;

/// <summary>
/// Provides a base class for all business logic models with a unique identifier.
/// </summary>
public abstract class AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the model.</param>
    protected AbstractModel(int id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the model.
    /// </summary>
    public int Id { get; set; }
}
