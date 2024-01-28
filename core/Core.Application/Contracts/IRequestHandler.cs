namespace Core.Application.Contracts;

public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    public Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
