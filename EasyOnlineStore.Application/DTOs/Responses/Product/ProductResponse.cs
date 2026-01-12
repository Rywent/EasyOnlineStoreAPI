namespace EasyOnlineStore.Application.DTOs.Responses.Product;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public decimal? OldPrice { get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<ProductImageResponse> Images { get; set; } = [];
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
}
