namespace EasyOnlineStore.Domain.Models.Carts;

public class Cart
{
    public Guid Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
}
