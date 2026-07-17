namespace EasyOnlineStore.Application.DTOs.Responses.User;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}