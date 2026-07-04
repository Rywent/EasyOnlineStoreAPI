namespace EasyOnlineStore.Domain.Models.Categories;

public class Category
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
}
