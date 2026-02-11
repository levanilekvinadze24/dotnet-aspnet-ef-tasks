namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categories")]
public class Category : BaseEntity
{
    public Category()
        : base()
    {
    }

    public Category(int id, string name)
        : base(id) => this.Name = name;

    [Required, MaxLength(200)]
    [Column("category_name")]
    public string Name { get; set; } = null!;

    [InverseProperty(nameof(ProductTitle.Category))]
    public virtual IList<ProductTitle> Titles { get; set; } = new List<ProductTitle>();
}
