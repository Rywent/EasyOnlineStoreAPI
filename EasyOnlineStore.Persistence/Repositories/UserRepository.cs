using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class UserRepository(EasyOnlineStoreDbContext context) : IUserRepository
{
    public async Task<List<ApplicationUser>> GetAllUsersAsync(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return await context.Users
            .AsNoTracking()
            .ToListAsync(ct);
    }
    public async Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<ApplicationUser?> GetByEmailWithPasswordHashAsync(string email, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }
    
    
}