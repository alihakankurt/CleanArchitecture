namespace Application.Features.Users.Exceptions;

public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid credentials have been used to authenticate")
    {
    }
}
