namespace EasyOnlineStore.Application.DTOs.Responses.Category;

public class CategoryResponse
{

    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
}
