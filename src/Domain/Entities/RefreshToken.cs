using Core.Domain.Contracts;
using Core.Domain.Entities;

namespace Domain.Entities;

public sealed class RefreshToken : Entity<int>, ICreationAuditable
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
