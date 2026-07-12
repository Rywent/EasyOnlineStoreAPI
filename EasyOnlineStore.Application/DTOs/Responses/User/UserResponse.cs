namespace EasyOnlineStore.Application.DTOs.Responses.User;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public string Country { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
}