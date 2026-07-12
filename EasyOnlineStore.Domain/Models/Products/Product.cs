using EasyOnlineStore.Domain.Models.Warehouses;
using EasyOnlineStore.Domain.Models.Categories;
using EasyOnlineStore.Domain.Models.Users;


namespace EasyOnlineStore.Domain.Models.Products;


public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription {  get; set; }
    public decimal? OldPrice { get; set; }
    public int Stock { get; set; } = 0;
    public string Sku { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public decimal Price { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<ProductImage> Images { get; set; } = [];
 
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    
    public Guid SellerId { get; set; }
    public ApplicationUser Seller { get; set; } = null!;
    
}
