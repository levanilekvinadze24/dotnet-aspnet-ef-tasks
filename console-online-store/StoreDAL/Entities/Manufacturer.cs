namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("manufacturers")]
public class Manufacturer : BaseEntity
{
    public Manufacturer()
        : base()
    {
    }

    public Manufacturer(int id, string name)
        : base(id) => this.Name = name;

    [Required, MaxLength(200)]
    [Column("manufacturer_name")]
    public string Name { get; set; } = null!;

    [InverseProperty(nameof(Product.Manufacturer))]
    public virtual IList<Product> Products { get; set; } = new List<Product>();
}
