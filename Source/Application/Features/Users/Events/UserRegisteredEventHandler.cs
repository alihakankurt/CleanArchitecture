using Core.Application.Events;
using Domain.Events;

namespace Application.Features.Users.Events;

public sealed class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
{
    public async ValueTask HandleAsync(UserRegisteredEvent domainEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"New User Registration: {domainEvent.UserId}");
    }
}
