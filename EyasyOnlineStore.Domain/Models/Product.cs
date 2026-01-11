namespace EasyOnlineStore.Domain.Models;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    public int Quantity { get; set; } = 0;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
}
