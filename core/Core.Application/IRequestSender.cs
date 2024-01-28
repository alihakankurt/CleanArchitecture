using Core.Application.Contracts;

namespace Core.Application;

public interface IRequestSender
{
    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
