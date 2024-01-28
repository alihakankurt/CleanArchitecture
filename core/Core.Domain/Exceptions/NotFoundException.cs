namespace Core.Domain.Exceptions;

public abstract class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
