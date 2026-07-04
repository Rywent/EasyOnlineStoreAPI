namespace EasyOnlineStore.Application.DTOs.Requests.User;

public class UpdateProfileRequest
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }   
    public string? Country { get; set; }
}