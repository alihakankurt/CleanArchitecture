using Core.Application.Services;

namespace Core.Infrastructure.Services;

public sealed class UtcDateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
