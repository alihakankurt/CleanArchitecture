using Core.Application.Requests;

namespace Core.Application;

/// <summary>
/// Represents base type of mediator implementation.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends the <paramref name="request"/> to the relevant handler to handle.
    /// </summary>
    public ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends the <paramref name="request"/> to the relevant handler to handle and gets back the response of type <typeparamref name="TResponse"/>.
    /// </summary>
    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
