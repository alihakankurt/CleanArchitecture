using Core.Domain.Entities;
using Domain.Events;

namespace Domain.Entities;

public sealed class User : Entity<Guid>, ICreationAuditable, IUpdateAuditable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";

    public string Email
    {
        get => field;
        set
        {
            RaiseEvent(new UserEmailChangedEvent(Id, field, value));
            field = value;
        }
    } = string.Empty;

    public byte[] PasswordHash { get; private set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; private set; } = Array.Empty<byte>();

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }

    public User(string firstName, string lastName, string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Id = Guid.CreateVersion7();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;

        RaiseEvent(new UserRegisteredEvent(Id));
    }

    private User()
    {
    }

    public void RecordLogin(DateTimeOffset now)
    {
        RaiseEvent(new UserLoggedInEvent(Id, now));
    }

    public void ChangePassword(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        RaiseEvent(new UserPasswordChangedEvent(Id));
    }

    public RefreshToken AddRefreshToken(string token, DateTimeOffset issuedAt, DateTimeOffset expiresAt)
    {
        var refreshToken = new RefreshToken(Id, token, issuedAt, expiresAt);
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void PruneRefreshTokens(DateTimeOffset now)
    {
        _refreshTokens.RemoveAll((token) => !token.IsActive(now));
    }
}
