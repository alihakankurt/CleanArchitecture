using Core.Application.Exceptions;

namespace Application.Features.Authentication.Exceptions;

public sealed class PasswordNotConfirmedException : BusinessRuleException
{
    public PasswordNotConfirmedException() : base("The confirmed password does not match with provided password")
    {
    }
}
