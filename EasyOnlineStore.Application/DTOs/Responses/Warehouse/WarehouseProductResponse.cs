namespace EasyOnlineStore.Application.DTOs.Responses.Warehouse;

public class WarehouseProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
