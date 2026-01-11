using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Domain.Models.Carts;

public class CartItem
{
    public Guid Id { get; set; }

    public Cart? Cart { get; set; }
    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
}
