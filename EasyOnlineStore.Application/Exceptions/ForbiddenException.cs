namespace EasyOnlineStore.Application.Exceptions;

public class ForbiddenException : BusinessException
{
    public ForbiddenException(string message) : base(message) { }

}