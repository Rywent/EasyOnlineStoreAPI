namespace EasyOnlineStore.Domain.Models.Products;

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Product Product { get; set; } = null!;
}
