namespace EasyOnlineStore.Application.DTOs.Requests.Product;

public class ProductImageUploadRequest
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}