namespace EasyOnlineStore.Application.DTOs.Responses.Warehouse;

public class WarehouseProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; } = string.Empty;

}
