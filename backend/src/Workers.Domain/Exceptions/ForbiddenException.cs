using Workers.Domain.Constants;

namespace Workers.Domain.Exceptions;

public class ForbiddenException(string message, string? code = null) 
    : DomainException(message)
{
    public override string Code => code ?? ErrorCodes.Forbidden;
}
