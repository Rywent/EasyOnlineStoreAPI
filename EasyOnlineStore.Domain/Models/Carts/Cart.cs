using EasyOnlineStore.Domain.Models.Users;

namespace EasyOnlineStore.Domain.Models.Carts;

public class Cart
{
    public Guid Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
    
    public Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
