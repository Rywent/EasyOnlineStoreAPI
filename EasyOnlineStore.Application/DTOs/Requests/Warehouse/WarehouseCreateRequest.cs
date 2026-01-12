namespace EasyOnlineStore.Application.DTOs.Requests.Warehouse;

public class WarehouseCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Adress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public decimal DeliveryCost { get; set; }

}
