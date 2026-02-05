using Workers.Domain.Constants;

namespace Workers.Domain.Exceptions;

public class NotFoundException(string entityName, object key) 
    : DomainException($"{entityName} with id '{key}' was not found.")
{
    public override string Code => ErrorCodes.NotFound;
    
    public string EntityName { get; } = entityName;
    public object Key { get; } = key;
}
