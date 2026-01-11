namespace EasyOnlineStore.Application.DTOs.Responses.Cart;

public class CartResponse
{
    public Guid CartId { get; set; }
    public List<CartItemResponse> CartItems { get; set; } = [];
    public decimal TotalPrice => CartItems.Select(c => c.SubTotal).Sum();
}
