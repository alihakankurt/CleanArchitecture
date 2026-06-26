using Core.Application.Requests;
using Core.Domain.Events;

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

    /// <summary>
    /// Publishes the <paramref name="domainEvent"/> to the relevant handlers to handle.
    /// </summary>
    public ValueTask PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
