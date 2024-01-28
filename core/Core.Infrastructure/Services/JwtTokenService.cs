using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Application.Models;
using Core.Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Infrastructure.Services;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtTokenOptions _tokenOptions;
    private readonly IDateTimeService _dateTimeService;

    public JwtTokenService(IOptions<JwtTokenOptions> tokenOptions, IDateTimeService dateTimeService)
    {
        _tokenOptions = tokenOptions.Value;
        _dateTimeService = dateTimeService;
    }

    public TokenModel GenerateAccessToken(string identity, string username, string email, IEnumerable<string>? roles = default)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, identity),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
        };

        if (roles is not null)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        DateTime now = _dateTimeService.Now;
        DateTime expires = now.AddMinutes(_tokenOptions.AccessTokenExpirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: claims.ToArray(),
            notBefore: now,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new TokenModel(new JwtSecurityTokenHandler().WriteToken(token), now, expires);
    }

    public TokenModel GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        Span<byte> bytes = stackalloc byte[64];
        rng.GetBytes(bytes);
        DateTime now = _dateTimeService.Now;
        DateTime expires = now.AddMinutes(_tokenOptions.RefreshTokenExpirationInMinutes);
        return new TokenModel(Convert.ToBase64String(bytes), now, expires);
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
