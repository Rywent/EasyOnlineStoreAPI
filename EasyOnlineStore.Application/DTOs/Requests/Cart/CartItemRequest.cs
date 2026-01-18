namespace EasyOnlineStore.Application.DTOs.Requests.Cart;

public class CartItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
