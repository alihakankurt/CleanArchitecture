namespace Core.Application.Requests;

/// <summary>
/// Represents the base type of requests.
/// </summary>
public interface IRequest
{
}

/// <summary>
/// Represents the base type of requests with a response of type <see cref="TResponse"/>.
/// </summary>
public interface IRequest<out TResponse>
{
}
