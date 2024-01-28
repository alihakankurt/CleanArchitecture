using Core.Application.Services;

namespace Core.Infrastructure.Services;

public sealed class LocalDateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
