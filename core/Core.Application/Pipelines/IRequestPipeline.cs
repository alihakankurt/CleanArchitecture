using Core.Application.Requests;

namespace Core.Application.Pipelines;

/// <summary>
/// Represents the call for next handler in pipeline with no response.
/// </summary>
public delegate ValueTask NextHandlerDelegate<TRequest>() where TRequest : IRequest;

/// <summary>
/// Represents the base type of request pipelines with no response.
/// </summary>
public interface IRequestPipeline<TRequest> where TRequest : IRequest
{
    /// <summary>
    /// Handles the incoming request.
    /// </summary>
    public ValueTask HandleAsync(TRequest request, NextHandlerDelegate<TRequest> next, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the call for next handler in pipeline with a response of type <see cref="TResponse"/>.
/// </summary>
public delegate ValueTask<TResponse> NextHandlerDelegate<TRequest, TResponse>() where TRequest : IRequest<TResponse>;

/// <summary>
/// Represents the base type of request pipelines with a response of type <see cref="TResponse"/>.
/// </summary>
public interface IRequestPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the incoming request.
    /// </summary>
    public ValueTask<TResponse> HandleAsync(TRequest request, NextHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default);
}
