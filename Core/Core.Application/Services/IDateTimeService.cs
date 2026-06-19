namespace Core.Application.Services;

/// <summary>
/// Represents an abstraction for time-related functionality.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Gets the current datetime.
    /// </summary>
    public DateTimeOffset Now { get; }

    /// <summary>
    /// Checks whether the <paramref name="datetime"/> is in past or not.
    /// </summary>
    public bool IsInPast(DateTimeOffset datetime)
    {
        return datetime < Now;
    }

    /// <summary>
    /// Checks whether the <paramref name="datetime"/> is in future or not.
    /// </summary>
    public bool IsInFuture(DateTimeOffset datetime)
    {
        return datetime > Now;
    }
}
