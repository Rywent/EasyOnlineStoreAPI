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

namespace EasyOnlineStore.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    UserManager<ApplicationUser> userManager,
    JwtProvider jwtProvider)
    : IUserService
{
    public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
            
        return mapper.Map<UserResponse>(user);
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllUsersAsync();
        return mapper.Map<List<UserResponse>>(users);
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        var user = await userRepository.GetByEmailWithPasswordHashAsync(email);
        if (user == null)
            throw new NotFoundException($"User with email {email} not found");
            
        return mapper.Map<UserResponse>(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailWithPasswordHashAsync(request.Email);
        if(user == null)
            throw new NotFoundException("User with email address not found");
        
        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if(!isPasswordValid)
            throw new ValidationException("Invalid password");
        
        var token = jwtProvider.GenerateToken(user);
        
        var loginResponse = mapper.Map<LoginResponse>(user);
        loginResponse.Token = token;
        
        return loginResponse;
    }

    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        var userEmail = request.Email;
        var existingUser = await userRepository.GetByEmailWithPasswordHashAsync(request.Email);
        if(existingUser != null)
            throw new ConflictException("Email", userEmail);
        
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
            throw new ValidationException($"Registration failed: {errors}");
        }
        
        return mapper.Map<UserResponse>(userEntity);
    }

    public async Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
            
        return mapper.Map<UserProfileResponse>(user);
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
        
        mapper.Map(request, user);
        
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new ValidationException($"Profile update failed: {errors}");
        }
        
        var updatedUser = await userRepository.GetByIdAsync(userId, ct);
        return mapper.Map<UserProfileResponse>(updatedUser);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"Password change failed: {errors}");
        }
        
        return true;
    }
    
    public async Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
    
        return user.Roles.Select(r => r.ToString()).ToList();
    }
    
    public async Task<bool> UpdateUserRolesAsync(Guid userId, List<string> newRoles, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
    
        var roles = new List<UserRole>();
        foreach (var roleName in newRoles)
        {
            if (!Enum.TryParse<UserRole>(roleName, true, out var role))
            {
                throw new ValidationException($"Invalid role: {roleName}. Available roles: {string.Join(", ", Enum.GetNames(typeof(UserRole)))}");
            }
            roles.Add(role);
        }
    
        user.Roles = roles;
    
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"Role update failed: {errors}");
        }
    
        return true;
    }

    public async Task<bool> LockUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
        
        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"Lock user failed: {errors}");
        }
        
        return true;
    }

    public async Task<bool> UnlockUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
        
        user.LockoutEnabled = false;
        user.LockoutEnd = null;
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"Unlock user failed: {errors}");
        }
        
        return true;
    }
    
    public async Task<bool> DeleteUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found");
    
        var result = await userManager.DeleteAsync(user);
    
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"User deletion failed: {errors}");
        }
    
        return true;
    }
}