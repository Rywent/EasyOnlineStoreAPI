namespace EasyOnlineStore.Application.DTOs.Responses.Order;

public class OrderItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid WarehouseId { get; set; }
    public decimal SubTotal => Quantity * UnitPrice;
}
