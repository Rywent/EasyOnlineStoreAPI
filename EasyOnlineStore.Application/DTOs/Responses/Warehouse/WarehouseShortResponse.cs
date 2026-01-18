namespace EasyOnlineStore.Application.DTOs.Responses.Warehouse;

public class WarehouseShortResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}
