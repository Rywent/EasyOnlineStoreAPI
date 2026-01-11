namespace EasyOnlineStore.Application.DTOs.Requests.Order;

public class OrderItemCreateRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
