namespace EasyOnlineStore.Domain.Models;

public class Warehouse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = [];
}
