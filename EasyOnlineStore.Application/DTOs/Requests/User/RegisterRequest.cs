namespace EasyOnlineStore.Application.DTOs.Requests.User;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string Role { get; set; } = string.Empty;
}