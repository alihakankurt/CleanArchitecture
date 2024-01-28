using Core.Application.Models;

namespace Core.Application.Services;

public interface ITokenService
{
    public TokenModel GenerateAccessToken(string identity, string username, string email, IEnumerable<string>? roles = default);

    public TokenModel GenerateRefreshToken();
}
