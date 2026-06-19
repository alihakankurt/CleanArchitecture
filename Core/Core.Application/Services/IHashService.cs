namespace Core.Application.Services;

/// <summary>
/// Represents an abstraction for generating and verifing password hashes.
/// </summary>
public interface IHashService
{
    /// <summary>
    /// Generates a new hash and salt using <paramref name="password"/>.
    /// </summary>
    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    /// <summary>
    /// Verifies the <paramref name="password"/> by using the hash and salt.
    /// </summary>
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}
