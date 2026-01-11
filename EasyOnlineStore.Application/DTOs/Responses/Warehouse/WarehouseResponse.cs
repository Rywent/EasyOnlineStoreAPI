using EasyOnlineStore.Application.DTOs.Responses.Product;

namespace EasyOnlineStore.Application.DTOs.Responses.Warehouse;

public class WarehouseResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<WarehouseProductResponse> Products { get; set; } = [];
}
