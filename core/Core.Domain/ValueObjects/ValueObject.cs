namespace Core.Domain.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected ValueObject()
    {
    }

    public abstract IEnumerable<object> GetValues();

    public bool Equals(ValueObject? other)
    {
        return other is not null && other.GetValues().SequenceEqual(GetValues());
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && Equals(other);
    }

    public override int GetHashCode()
    {
        int hashCode = 0;
        foreach (var value in GetValues())
        {
            hashCode = HashCode.Combine(hashCode, value);
        }

        return hashCode;
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
