using System.ComponentModel.DataAnnotations;
using System.Data;
using AutoMapper;
using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Users;
using EasyOnlineStore.Infrastructure.Jwt;
using Microsoft.AspNetCore.Identity;

namespace EasyOnlineStore.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    

    public UserService(IUserRepository userRepository, IMapper mapper, UserManager<ApplicationUser> userManager, JwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if(user is null)
            throw new NotFoundException("User with email address not found");
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if(!isPasswordValid)
            throw new ValidationException("Invalid password");
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }

    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        var userEmail = request.Email;
        var isEmailUnique = await _userManager.FindByEmailAsync(request.Email);
        if(isEmailUnique is not null)
            throw new ConflictException("Email", userEmail);
        
        var userEntity = _mapper.Map<ApplicationUser>(request);
        var createdUser = await _userManager.CreateAsync(userEntity, request.Password);
        
        if (!createdUser.Succeeded)
        {
            var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
            throw new ValidationException($"Registration failed: {errors}");
        }
        
        return _mapper.Map<UserResponse>(userEntity);
    }

    public async Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string password, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}