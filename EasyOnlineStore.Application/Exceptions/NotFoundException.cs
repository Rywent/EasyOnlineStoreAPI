namespace EasyOnlineStore.Application.Exceptions;

public class NotFoundException : BusinessException
{
    public NotFoundException(string entityName, Guid id) : base($"{entityName} with ID '{id} not found") { }
    public NotFoundException(string message) : base(message) { }
}
