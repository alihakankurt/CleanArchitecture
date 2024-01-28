using Domain.Entities;

namespace Application.Models.Responses;

public sealed record UserResponseModel(int Id, string FirstName, string LastName, string Email, DateTime CreatedAt, DateTime UpdatedAt)
{
    public static explicit operator UserResponseModel(User user)
    {
        return new UserResponseModel(user.Id, user.FirstName, user.LastName, user.Email, user.CreatedAt, user.UpdatedAt);
    }
}
