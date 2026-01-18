namespace EasyOnlineStore.Application.DTOs.Requests.Warehouse;

public class WarehouseUpdateRequest
{
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Adress { get; set; }
    public string? Phone { get; set; }
    public bool? IsActive { get; set; }
    public decimal? DeliveryCost { get; set; }
}
