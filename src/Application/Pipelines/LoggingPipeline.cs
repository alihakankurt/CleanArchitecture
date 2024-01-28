using Core.Application.Contracts;

namespace Application.Pipelines;

internal sealed class LoggingPipeline<TRequest, TResponse> : IRequestPipeline<TRequest, TResponse>
    where TRequest : notnull
{
    public Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Handling {request}");
        return next(request, cancellationToken);
    }
}
