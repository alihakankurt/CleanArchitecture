namespace Application.Features.Users.Dtos;

public sealed class UserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
