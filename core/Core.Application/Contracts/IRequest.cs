namespace Core.Application.Contracts;

public interface IRequest
{
}

public interface IRequest<out TResponse>
{
}
