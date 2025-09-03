namespace Application.Features.Users.Dtos;

public sealed class UserLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
