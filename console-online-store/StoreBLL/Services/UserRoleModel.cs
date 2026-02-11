namespace StoreBLL.Models;

/// <summary>
/// Represents a role assigned to a user.
/// </summary>
public class UserRoleModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the role.</param>
    /// <param name="roleName">The name of the role.</param>
    public UserRoleModel(int id, string roleName)
        : base(id)
    {
        this.RoleName = roleName;
    }

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string RoleName { get; set; }
}
