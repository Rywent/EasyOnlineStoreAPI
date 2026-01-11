namespace EasyOnlineStore.Application.DTOs.Responses.Cart;

public class CartItemResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal => UnitPrice * Quantity;
}
