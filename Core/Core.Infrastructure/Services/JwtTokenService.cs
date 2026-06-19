using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Infrastructure.Services;

/// <summary>
/// Represents the implementation of <see cref="ITokenService"/> using JSON Web Tokens.
/// </summary>
public sealed class JwtTokenService : ITokenService
{
    private readonly JwtTokenOptions _tokenOptions;
    private readonly IDateTimeService _dateTimeService;

    public JwtTokenService(IOptions<JwtTokenOptions> tokenOptions, IDateTimeService dateTimeService)
    {
        _tokenOptions = tokenOptions.Value;
        _dateTimeService = dateTimeService;
    }

    public TokenInfo GenerateAccessToken(string identity, string username, string email, params ReadOnlySpan<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, identity),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        DateTimeOffset createdAt = _dateTimeService.Now;
        DateTimeOffset expiresAt = createdAt.AddMinutes(_tokenOptions.AccessTokenExpirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims.ToArray(),
            notBefore: createdAt.LocalDateTime,
            expires: expiresAt.LocalDateTime,
            signingCredentials: signingCredentials
        );

        return new TokenInfo
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            CreatedAt = createdAt,
            ExpiresAt = expiresAt
        };
    }

    public TokenInfo GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        Span<byte> bytes = stackalloc byte[64];
        rng.GetBytes(bytes);
        DateTimeOffset createdAt = _dateTimeService.Now;
        DateTimeOffset expiresAt = createdAt.AddMinutes(_tokenOptions.RefreshTokenExpirationInMinutes);
        return new TokenInfo
        {
            Token = Convert.ToBase64String(bytes),
            CreatedAt = createdAt,
            ExpiresAt = expiresAt
        };
    }

    public sealed class JwtTokenOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int AccessTokenExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInMinutes { get; set; }
    }
}
