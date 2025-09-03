using Core.Domain.Entities;

namespace Domain.Entities;

public sealed class RefreshToken : Entity
{
    public long UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
