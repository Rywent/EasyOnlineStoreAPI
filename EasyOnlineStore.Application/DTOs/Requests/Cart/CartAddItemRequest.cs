namespace EasyOnlineStore.Application.DTOs.Requests.Cart;

public class CartAddItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
