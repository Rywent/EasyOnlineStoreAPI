using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;


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
        return BadRequest("User not found");
    }

    // POST api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
    {
        var result = await userService.LoginAsync(request);
        return Ok(result);
    }
}