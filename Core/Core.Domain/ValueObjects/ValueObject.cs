using System.Numerics;

namespace Core.Domain.ValueObjects;

/// <summary>
/// Represents the base type of value objects.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>, IEqualityOperators<ValueObject, ValueObject, bool>
{
    /// <summary>
    /// Gets the values inside the object.
    /// </summary>
    public abstract IEnumerable<object> Values { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ValueObject"/>.
    /// </summary>
    protected ValueObject()
    {
    }

    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        return other?.Values.SequenceEqual(Values) ?? false;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Values
            .Select(static (value) => value?.GetHashCode() ?? 0)
            .Aggregate(static (acc, hash) => acc ^ hash);
    }

    /// <inheritdoc />
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null || right is null)
        {
            return left is null && right is null;
        }

        return left?.Equals(right) ?? false;
    }

    /// <inheritdoc />
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
