using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EasyOnlineStore.API.Controllers;

[EnableRateLimiting("auth")]
public class AuthController(IUserService userService, ICartService cartService) : BaseApiController
{
    // POST api/auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        var result = await userService.RegisterAsync(request);
        if (result != null)
        {
            await cartService.CreateCartAsync(result.Id);
            return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
        }
        return BadRequest("Registration failed");
    }

    // POST api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
    {
        var result = await userService.LoginAsync(request);
        return Ok(result);
    }
    
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.RefreshToken))
            return BadRequest("Refresh token is required");
        
        var result = await userService.RefreshTokenAsync(request.RefreshToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        var userId = GetUserIdFromToken();
        await userService.LogoutAsync(userId, request.RefreshToken);
        return Ok(new { message = "Logged out successfully" });
    }
}