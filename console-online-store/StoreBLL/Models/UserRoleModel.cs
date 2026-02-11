namespace StoreBLL.Models;

/// <summary>
/// Represents a user of the store application.
/// </summary>
public class UserModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserModel"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="login">The login name of the user.</param>
    /// <param name="roleId">The identifier of the role assigned to the user.</param>
    /// <param name="roleName">The descriptive name of the role.</param>
    public UserModel(int id, string firstName, string lastName, string login, int roleId, string roleName)
        : base(id)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Login = login;
        this.RoleId = roleId;
        this.RoleName = roleName;
    }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the login name of the user.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user's role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the descriptive name of the user's role.
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// Gets or sets the user's password (used only when adding or updating from UI).
    /// </summary>
    public string? Password { get; set; }
}
