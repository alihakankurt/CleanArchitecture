namespace Core.Application.Contracts;

public delegate Task<TResponse> RequestHandlerDelegate<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : notnull;

public interface IRequestPipeline<TRequest, TResponse> where TRequest : notnull
{
    public Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default);
}
