namespace EasyOnlineStore.Domain.Models;

public class Cart
{
    public Guid Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
}
