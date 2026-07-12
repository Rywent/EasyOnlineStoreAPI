using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;

namespace EasyOnlineStore.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetUserByIdAsync(Guid userId);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse?> GetUserByEmailAsync(string email);
    Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
    
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
    Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task<bool> ChangePasswordAsync(Guid userId, string password, CancellationToken ct = default);

    public Task<bool> UpdateUserRolesAsync(Guid userId, List<string> newRoles, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> LockUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> UnlockUserAsync(Guid userId, CancellationToken ct = default);
    
}