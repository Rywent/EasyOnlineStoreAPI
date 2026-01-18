namespace EasyOnlineStore.Application.DTOs.Requests.Product;

public class ProductCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public decimal? OldPrice { get; set; }
    public int Stock { get; set; } = 0;
    public decimal Price { get; set; } = 0;
    public List<ProductImageRequest> Images { get; set; } = [];
    public Guid CategoryId { get; set; }
    public Guid WarehouseId { get; set; }
}
