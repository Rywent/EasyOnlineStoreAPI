namespace EasyOnlineStore.Application.Exceptions;

public class ConflictException : BusinessException
{
    public string ResourceType { get; }
    public string ConflictField { get; }

    public ConflictException(string resourceType, string field, string? customMessage = null)
        : base(customMessage ?? $"{resourceType} with {field} already exists.")
    {
        ResourceType = resourceType;
        ConflictField = field;
    }
}