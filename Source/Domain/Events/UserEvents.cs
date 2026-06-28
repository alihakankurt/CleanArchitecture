using Core.Domain.Events;

namespace Domain.Events;

public record UserRegisteredEvent(Guid UserId) : IDomainEvent;

public record UserLoggedInEvent(Guid UserId, DateTimeOffset LoginDate) : IDomainEvent;

public record UserEmailChangedEvent(Guid UserId, string OldEmail, string NewEmail) : IDomainEvent;

public record UserPasswordChangedEvent(Guid UserId) : IDomainEvent;
