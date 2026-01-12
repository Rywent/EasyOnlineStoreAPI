namespace EasyOnlineStore.Application.DTOs.Responses.Warehouse;

public class WarehouseResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Adress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal DeliveryCost { get; set; }
    public int LikesCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WarehouseProductResponse> Products { get; set; } = [];
}
