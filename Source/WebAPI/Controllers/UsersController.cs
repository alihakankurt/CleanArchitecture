using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Application.Features.Users.Dtos;
using Core.Application;
using Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/users")]
public sealed class UsersController : ApiControllerBase
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    [ProducesResponseType<TokenInfo>(StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var response = await Mediator.SendAsync(new RegisterCommand(userRegistrationDto));
        SetRefreshToken(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpPost("login")]
    [ProducesResponseType<TokenInfo>(StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var response = await Mediator.SendAsync(new LoginCommand(userLoginDto));
        SetRefreshToken(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [Authorize]
    [HttpPatch("refresh")]
    [ProducesResponseType<TokenInfo>(StatusCodes.Status200OK)]
    public async ValueTask<IActionResult> Refresh()
    {
        if (GetRefreshToken() is not string refreshToken)
            return Unauthorized();

        var response = await Mediator.SendAsync(new RefreshCommand(refreshToken));
        return Ok(response.AccessToken);
    }

    [Authorize]
    [HttpPatch("revoke")]
    public async ValueTask<IActionResult> Revoke()
    {
        if (GetRefreshToken() is not string refreshToken)
            return Unauthorized();

        await Mediator.SendAsync(new RevokeCommand(refreshToken));
        RemoveRefreshToken();
        return Ok();
    }

    [Authorize]
    [HttpGet("current-user")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CurrentUser()
    {
        if (GetRefreshToken() is not string refreshToken)
            return Unauthorized();

        var response = await Mediator.SendAsync(new GetCurrentUserQuery(CurrentUserId));
        return Ok(response);
    }
}
