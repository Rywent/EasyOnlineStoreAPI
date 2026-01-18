namespace EasyOnlineStore.Application.DTOs.Requests.Cart;

public class CartItemUpdateRequest
{   
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
