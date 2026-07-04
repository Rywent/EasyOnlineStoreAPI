using EasyOnlineStore.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<ApplicationUser?> GetByEmailWithPasswordHashAsync(string email, CancellationToken ct = default); 
    
}