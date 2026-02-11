namespace StoreDAL.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    protected BaseEntity(int id) => this.Id = id;

    protected BaseEntity() => this.Id = 0;

    [Key]
    [Column("id")]
    public int Id { get; set; }
}
