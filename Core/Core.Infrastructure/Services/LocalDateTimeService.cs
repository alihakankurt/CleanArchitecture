using Core.Application.Services;

namespace Core.Infrastructure.Services;

/// <summary>
/// Represents the implementation of <see cref="IDateTimeService"/> using local time.
/// </summary>
public sealed class LocalDateTimeService : IDateTimeService
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
