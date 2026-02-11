namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User : BaseEntity
{
    public User()
        : base()
    {
    }

    public User(int id, string name, string lastName, string login, string password, int roleId)
        : base(id)
    {
        this.Name = name;
        this.LastName = lastName;
        this.Login = login;
        this.Password = password;
        this.RoleId = roleId;
    }

    [Required, MaxLength(100)]
    [Column("first_name")]
    public string Name { get; set; } = null!;

    [Required, MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Required, MaxLength(100)]
    [Column("login")]
    public string Login { get; set; } = null!;

    [Required, MaxLength(256)]
    [Column("Password")]
    public string Password { get; set; } = null!;

    [Column("user_role_id")]
    public int RoleId { get; set; }

    // This name ("Role") is what UserRole.Users points to via InverseProperty
    [ForeignKey(nameof(RoleId))]
    public UserRole Role { get; set; } = null!;

    [InverseProperty(nameof(CustomerOrder.User))]
    public virtual IList<CustomerOrder> Order { get; set; } = new List<CustomerOrder>();
}
