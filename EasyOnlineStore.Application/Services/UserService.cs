using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Users;
using EasyOnlineStore.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    UserManager<ApplicationUser> userManager,
    JwtProvider jwtProvider,
    ILogger<UserService> logger)
    : IUserService
{
    public async Task<UserResponse?> GetUserByIdAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
        
        return mapper.Map<UserResponse>(user);
    }

    public async Task<List<UserResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var users = await userRepository.GetUsersByPageAsync(page, pageSize, ct);
        return mapper.Map<List<UserResponse>>(users);
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailWithPasswordHashAsync(email, ct);
        if (user == null)
        {
            LogUserByEmailNotFound(logger, email);
            throw new NotFoundException($"User with email {email} not found");
        }
            
        return mapper.Map<UserResponse>(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailWithPasswordHashAsync(request.Email, ct);
        if (user == null)
        {
            LogLoginFailedUserNotFound(logger, request.Email);
            throw new NotFoundException("User with email address not found");
        }
        
        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            LogLoginFailedInvalidPassword(logger, request.Email, user.Id);
            throw new ValidationException("Invalid password");
        }
        
        var token = jwtProvider.GenerateToken(user);
        
        var loginResponse = mapper.Map<LoginResponse>(user);
        loginResponse.Token = token;
        
        LogUserLoggedIn(logger, request.Email, user.Id);
        return loginResponse;
    }

    public async Task<UserResponse?> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var userEmail = request.Email;
        var existingUser = await userRepository.GetByEmailWithPasswordHashAsync(request.Email, ct);
        if (existingUser != null)
        {
            LogUserAlreadyExists(logger, request.Email, existingUser.Id);
            throw new ConflictException("Email", userEmail);
        }
        
        var userEntity = mapper.Map<ApplicationUser>(request);
        
        var userRoles = new List<UserRole>();
        if (request.Roles.Any())
        {
            foreach (var roleName in request.Roles)
            {
                if (Enum.TryParse<UserRole>(roleName, true, out var role))
                {
                    userRoles.Add(role);
                }
                else
                {
                    LogRegistrationFailedInvalidRole(logger, roleName);
                    throw new ValidationException($"Invalid role name: {roleName}. Available roles: {string.Join(", ", Enum.GetNames(typeof(UserRole)))}");
                }
            }
        }
        else
        {
            userRoles.Add(UserRole.Customer);
        }
        
        userEntity.Roles = userRoles;
        
        var createdUser = await userManager.CreateAsync(userEntity, request.Password);
        
        if (!createdUser.Succeeded)
        {
            var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
            LogRegistrationFailed(logger, errors, userEntity.Id);
            throw new ValidationException($"Registration failed: {errors}");
        }
        
        LogUserRegistered(logger, userEntity.Email, userEntity.Id);
        return mapper.Map<UserResponse>(userEntity);
    }

    public async Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
            
        return mapper.Map<UserProfileResponse>(user);
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
        
        mapper.Map(request, user);
        
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            LogProfileUpdateFailed(logger, errors, userId);
            throw new ValidationException($"Profile update failed: {errors}");
        }
        
        var updatedUser = await userRepository.GetByIdAsync(userId, ct);
        LogProfileUpdated(logger, userId);
        return mapper.Map<UserProfileResponse>(updatedUser);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            LogPasswordResetFailed(logger, errors, userId);
            throw new ValidationException($"Password change failed: {errors}");
        }
        
        LogPasswordChanged(logger, userId);
        return true;
    }
    
    public async Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
    
        return user.Roles.Select(r => r.ToString()).ToList();
    }
    
    public async Task<bool> UpdateUserRolesAsync(Guid userId, List<string> newRoles, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
    
        var roles = new List<UserRole>();
        foreach (var roleName in newRoles)
        {
            if (!Enum.TryParse<UserRole>(roleName, true, out var role))
            {
                LogRoleUpdateFailedInvalidRole(logger, roleName, userId);
                throw new ValidationException($"Invalid role: {roleName}. Available roles: {string.Join(", ", Enum.GetNames(typeof(UserRole)))}");
            }
            roles.Add(role);
        }
    
        user.Roles = roles;
    
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            LogRoleUpdateFailed(logger, errors, userId);
            throw new ValidationException($"Role update failed: {errors}");
        }

        LogUserRolesUpdated(logger, userId);
        return true;
    }

    public async Task<bool> LockUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
        
        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            LogLockUserFailed(logger, errors, userId);
            throw new ValidationException($"Lock user failed: {errors}");
        }
        
        LogUserLocked(logger, userId);
        return true;
    }

    public async Task<bool> UnlockUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
        
        user.LockoutEnabled = false;
        user.LockoutEnd = null;
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            LogUnlockUserFailed(logger, errors, userId);
            throw new ValidationException($"Unlock user failed: {errors}");
        }
        
        LogUserUnlocked(logger, userId);
        return true;
    }
    
    public async Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            LogUserNotFound(logger, userId);
            throw new NotFoundException($"User with ID {userId} not found");
        }
    
        var result = await userManager.DeleteAsync(user);
    
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            LogUserDeletionFailed(logger, errors, userId);
            throw new ValidationException($"User deletion failed: {errors}");
        }
    
        LogUserDeleted(logger, userId);
        return true;
    }
}