using Core.Application;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class DomainEventInterceptor<TId> : SaveChangesInterceptor where TId : notnull
{
    private readonly IMediator _mediator;

    public DomainEventInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        InterceptionResult<int> interceptionResult = base.SavingChanges(eventData, result);
        AuditCreation(eventData.Context).GetAwaiter().GetResult();
        return interceptionResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        InterceptionResult<int> interceptionResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        await AuditCreation(eventData.Context, cancellationToken);
        return interceptionResult;
    }

    private async ValueTask AuditCreation(DbContext? context, CancellationToken cancellationToken = default)
    {
        if (context is null)
            return;

        var domainEvents = context.ChangeTracker.Entries<Entity<TId>>()
            .SelectMany(static (entry) => entry.Entity.ReleaseDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.PublishAsync(domainEvent, cancellationToken);
        }
    }
}
