namespace StoreBLL.Security;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides functionality for hashing passwords with a fixed salt using SHA-256.
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Salt value applied to passwords before hashing.
    /// </summary>
    private const string Salt = "store-salt"; // demo salt

    /// <summary>
    /// Hashes a password using SHA-256 with a fixed salt.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>The hashed password as an uppercase hexadecimal string.</returns>
    public static string Hash(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(Salt + password);
        return Convert.ToHexString(SHA256.HashData(bytes)); // UPPER HEX, efficient
    }
}
