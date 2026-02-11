namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("product_titles")]
public class ProductTitle : BaseEntity
{
    public ProductTitle()
        : base()
    {
    }

    public ProductTitle(int id, string title, int categoryId)
        : base(id)
    {
        this.Title = title;
        this.CategoryId = categoryId;
    }

    [Required, MaxLength(300)]
    [Column("product_title")]
    public string Title { get; set; } = null!;

    [Column("category_id")]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;

    [InverseProperty(nameof(Product.Title))]
    public virtual IList<Product> Products { get; set; } = new List<Product>();
}
