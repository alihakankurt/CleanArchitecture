using Core.Application.Pipelines;
using Core.Application.Requests;

namespace Application.Pipelines;

internal sealed class LoggingPipeline<TRequest> : IRequestPipeline<TRequest>
    where TRequest : IRequest
{
    public async ValueTask HandleAsync(TRequest request, NextHandlerDelegate<TRequest> next, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Handling {request}");
        await next();
    }
}

internal sealed class LoggingPipeline<TRequest, TResponse> : IRequestPipeline<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async ValueTask<TResponse> HandleAsync(TRequest request, NextHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Handling {request}");
        return await next();
    }
}
