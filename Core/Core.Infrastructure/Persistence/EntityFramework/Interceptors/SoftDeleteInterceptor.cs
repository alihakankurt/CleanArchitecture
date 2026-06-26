using Core.Application.Services;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Infrastructure.Persistence.EntityFramework.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeService _dateTimeService;

    public SoftDeleteInterceptor(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        InterceptionResult<int> interceptionResult = base.SavingChanges(eventData, result);
        ApplySoftDeletion(eventData.Context);
        return interceptionResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        InterceptionResult<int> interceptionResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        ApplySoftDeletion(eventData.Context);
        return interceptionResult;
    }

    private void ApplySoftDeletion(DbContext? context)
    {
        if (context is null)
            return;

        DateTimeOffset now = _dateTimeService.Now;
        var entries = from entry in context.ChangeTracker.Entries<ISoftDeletable>()
                      where entry.State is EntityState.Deleted && !entry.Entity.IsDeleted
                      select entry;

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.DeletedAt = now;
        }
    }
}
