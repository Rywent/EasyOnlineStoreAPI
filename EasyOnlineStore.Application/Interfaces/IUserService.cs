using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;

namespace EasyOnlineStore.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetUserByIdAsync(Guid userId);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse?> GetUserByEmailAsync(string email);
    
    Task<string> LoginAsync(LoginRequest request);
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
    Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task<bool> ChangePasswordAsync(Guid userId, string password, CancellationToken ct = default);
}