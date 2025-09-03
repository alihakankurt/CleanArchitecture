namespace Core.Application.BusinessRules;

/// <summary>
/// Represents the base type of business rule related exceptions.
/// </summary>
public abstract class BusinessRuleException : Exception
{
    protected BusinessRuleException(string message) : base(message)
    {
    }
}
