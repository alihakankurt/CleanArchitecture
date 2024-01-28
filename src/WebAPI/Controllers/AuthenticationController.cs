using Application.Features.Authentication.Commands;
using Application.Features.Authentication.Queries;
using Application.Models.Requests;
using Core.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/auth")]
public sealed class AuthenticationController : ApiControllerBase
{
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    public async ValueTask<IActionResult> Register([FromBody] RegisterRequestModel registerRequestModel)
    {
        var response = await Mediator.SendAsync(new RegisterCommand(registerRequestModel, GetIpAddress()));
        SetRefreshToken(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpPost("login")]
    public async ValueTask<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
    {
        var response = await Mediator.SendAsync(new LoginCommand(loginRequestModel, GetIpAddress()));
        SetRefreshToken(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpGet("refresh")]
    public async ValueTask<IActionResult> Refresh()
    {
        var refreshToken = GetRefreshToken();
        if (refreshToken is null)
            return Unauthorized();

        var response = await Mediator.SendAsync(new RefreshCommand(refreshToken, GetIpAddress()));
        SetRefreshToken(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpPost("revoke")]
    public async ValueTask<IActionResult> Refresh([FromBody] string? refreshToken = default)
    {
        refreshToken ??= GetRefreshToken();
        if (refreshToken is null)
            return Unauthorized();

        await Mediator.SendAsync(new RevokeCommand(refreshToken));
        RemoveRefreshToken();
        return Ok();
    }

    [Authorize]
    [HttpGet("currentUser")]
    public async Task<IActionResult> CurrentUser()
    {
        var response = await Mediator.SendAsync(new GetUserByIdQuery(CurrentUserId));
        return Ok(response);
    }
}
