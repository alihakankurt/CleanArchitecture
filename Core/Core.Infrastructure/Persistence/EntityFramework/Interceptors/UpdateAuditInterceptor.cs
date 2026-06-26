using Core.Application.Services;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class UpdateAuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeService _dateTimeService;

    public UpdateAuditInterceptor(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        InterceptionResult<int> interceptionResult = base.SavingChanges(eventData, result);
        AuditUpdate(eventData.Context);
        return interceptionResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        InterceptionResult<int> interceptionResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        AuditUpdate(eventData.Context);
        return interceptionResult;
    }

    private void AuditUpdate(DbContext? context)
    {
        if (context is null)
            return;

        DateTimeOffset now = _dateTimeService.Now;
        var entries = from entry in context.ChangeTracker.Entries<IUpdateAuditable>()
                      where entry.State is EntityState.Modified or EntityState.Added
                      select entry;

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = now;
        }
    }
}
