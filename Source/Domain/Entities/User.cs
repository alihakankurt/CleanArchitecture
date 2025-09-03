using Core.Domain.Entities;

namespace Domain.Entities;

public sealed class User : Entity<long>, ICreationAuditable, IUpdateAuditable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();

    public void RemoveExpiredRefreshTokens(DateTimeOffset now)
    {
        RefreshTokens.RemoveAll((token) => token.ExpiresAt <= now);
    }
}
