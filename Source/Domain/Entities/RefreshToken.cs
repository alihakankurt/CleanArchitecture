using Core.Domain.Entities;

namespace Domain.Entities;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; }
    public string Token { get; } = string.Empty;
    public DateTimeOffset IssuedAt { get; }
    public DateTimeOffset ExpiresAt { get; }
    public bool IsRevoked { get; private set; }

    internal RefreshToken(Guid userId, string token, DateTimeOffset issuedAt, DateTimeOffset expiresAt)
    {
        Token = token;
        IssuedAt = issuedAt;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    private RefreshToken()
    {
    }

    public bool IsExpired(DateTimeOffset now)
    {
        return ExpiresAt < now;
    }

    public bool IsActive(DateTimeOffset now)
    {
        return !IsExpired(now) && !IsRevoked;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
