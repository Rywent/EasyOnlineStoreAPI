using EasyOnlineStore.Domain.Models.Products;

namespace EasyOnlineStore.Application.Exceptions;

public class InsufficientStockException : BusinessException
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public Guid WarehouseId { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }

    public InsufficientStockException(Product product, int requestedQuantity)
        : base($"Product '{product.Name}' (ID: {product.Id}) on warehouse '{product.WarehouseId}': requested {requestedQuantity}, available {product.Quantity}")
    {
        ProductId = product.Id;
        ProductName = product.Name;
        WarehouseId = product.WarehouseId;
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = product.Quantity;
    }
}
