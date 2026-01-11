namespace EasyOnlineStore.Application.Exceptions;

public class CartEmptyException : BusinessException
{
    public CartEmptyException(Guid cartId) : base($"Cart {cartId} is empty. Add items before creating order.") { }
}
