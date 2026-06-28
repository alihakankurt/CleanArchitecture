using System.Security.Claims;
using Core.Application;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private const string RefreshTokenKey = "refreshToken";

    protected IMediator Mediator { get; set; }
    protected Guid CurrentUserId => Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected void SetRefreshToken(TokenInfo tokenInfo)
    {
        HttpContext.Response.Cookies.Append(RefreshTokenKey, tokenInfo.Token, new CookieOptions
        {
            Expires = tokenInfo.ExpiresAt,
            HttpOnly = true,
        });
    }

    protected string? GetRefreshToken()
    {
        _ = HttpContext.Request.Cookies.TryGetValue(RefreshTokenKey, out string? refreshToken);
        return refreshToken;
    }

    protected void RemoveRefreshToken()
    {
        HttpContext.Response.Cookies.Delete(RefreshTokenKey);
    }

    protected string GetIpAddress()
    {
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var ipAddress))
            return ipAddress.ToString();

        return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
    }
}
