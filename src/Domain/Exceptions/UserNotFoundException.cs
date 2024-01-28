using Core.Domain.Exceptions;

namespace Domain.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(int id) : base($"User could not be found with id '{id}'")
    {
    }

    public UserNotFoundException(string email) : base($"User could not be found with email '{email}'")
    {
    }
}
