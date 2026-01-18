namespace EasyOnlineStore.Application.DTOs.Requests.Product;

public class ProductUpdateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public decimal? OldPrice { get; set; }
    public int? Stock { get; set; }
    public decimal? Price { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? WarehouseId { get; set; }
}
