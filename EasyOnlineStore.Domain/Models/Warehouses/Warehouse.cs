using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Users;

namespace EasyOnlineStore.Domain.Models.Warehouses;

public class Warehouse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public decimal DeliveryCost { get; set; }
    public int LikesCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Product> Products { get; set; } = [];
    

    public Guid OwnerUserId { get; set; }
    public ApplicationUser OwnerUser { get; set; } = null!;
}
