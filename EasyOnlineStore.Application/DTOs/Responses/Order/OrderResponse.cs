using EasyOnlineStore.Domain.Enums;

namespace EasyOnlineStore.Application.DTOs.Responses.Order;

public class OrderResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemResponse> Items { get; set; } = [];
    public decimal TotalPrice { get; set; }
}
