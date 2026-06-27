using Core.Application.Services;
using Core.Domain.Entities;
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
        InterceptionResult<int> interceptionResult = base.SavingChanges(eventData, result);
        AuditCreation(eventData.Context);
        return interceptionResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        InterceptionResult<int> interceptionResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        AuditCreation(eventData.Context);
        return interceptionResult;
    }

    private void AuditCreation(DbContext? context)
    {
        if (context is null)
            return;

        DateTimeOffset now = _dateTimeService.Now;
        var entries = from entry in context.ChangeTracker.Entries<ICreationAuditable>()
                      where entry.State is EntityState.Added
                      select entry;

        foreach (var entry in entries)
        {
            entry.Property(static (entity) => entity.CreatedAt).CurrentValue = now;
        }
    }
}
