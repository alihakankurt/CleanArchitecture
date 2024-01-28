using Core.Application.Services;
using Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class CreationAuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeService _dateTimeService;

    public CreationAuditInterceptor(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AuditCreation(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        AuditCreation(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AuditCreation(DbContext? context)
    {
        if (context is null)
            return;

        DateTime now = _dateTimeService.Now;
        var entries = from entry in context.ChangeTracker.Entries<ICreationAuditable>()
                      where entry.State is EntityState.Added
                      select entry;

        foreach (var entry in entries)
        {
            entry.Entity.CreatedAt = now;
        }
    }
}
