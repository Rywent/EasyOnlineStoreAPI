using EasyOnlineStore.Domain.Enums;

namespace EasyOnlineStore.Application.Exceptions;

public class InvalidOrderStatusException : BusinessException
{
    public OrderStatus CurrentStatus { get; }
    public InvalidOrderStatusException(OrderStatus status, string operation)
        : base($"Cannot {operation} order with status: {status}")
    {
        CurrentStatus = status;
    }
}
