using EasyOnlineStore.Domain.Enums;

namespace EasyOnlineStore.Domain.Models;

public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<OrderItem> Items { get; set; } = [];
}
