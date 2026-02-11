namespace StoreDAL.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("user_roles")]
public class UserRole : BaseEntity
{
    public UserRole()
    {
    }

    public UserRole(int id, string roleName)
        : base(id)
        => this.RoleName = roleName;

    [Required]
    [Column("user_role_name")]
    public string RoleName { get; set; } = null!;

    // Points to the navigation on User named "Role"
    [InverseProperty(nameof(User.Role))]
    public virtual IList<User> Users { get; set; } = new List<User>();
}
