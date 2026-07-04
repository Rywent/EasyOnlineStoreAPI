using Microsoft.AspNetCore.Identity;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models.Orders;

namespace EasyOnlineStore.Domain.Models.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    
    public UserRole Role { get; set; } = UserRole.Seller;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public List<Order>? Orders { get; set; } = new();
}