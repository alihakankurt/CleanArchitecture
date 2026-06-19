namespace Core.Application.Requests;

/// <summary>
/// Represents the base type of request handlers to handle requests with no response.
/// </summary>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    public ValueTask HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the base type of request handlers to handle requests with a response of type <see cref="TResponse"/>.
/// </summary>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
