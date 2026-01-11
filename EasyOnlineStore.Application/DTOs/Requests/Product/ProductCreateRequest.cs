namespace EasyOnlineStore.Application.DTOs.Requests.Product;

public class ProductCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid WarehouseId { get; set; }
}
