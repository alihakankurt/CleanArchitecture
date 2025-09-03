using Core.Application.Services;

namespace Core.Infrastructure.Services;

/// <summary>
/// Represents the implementation of <see cref="IDateTimeService"/> using Coordinated Universal Time.
/// </summary>
public sealed class UtcDateTimeService : IDateTimeService
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
