using Workers.Domain.Constants;

namespace Workers.Domain.Exceptions;

public class UnauthorizedException(string message = "Unauthorized access.", string? code = null) 
    : DomainException(message)
{
    public override string Code => code ?? ErrorCodes.Unauthorized;
}
