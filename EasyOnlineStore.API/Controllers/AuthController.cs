using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ICartService cartService) : ControllerBase
{
    
    // POST api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(RegisterRequest request)
    {
        var result = await userService.RegisterAsync(request);
        if (result != null)
        {
            await cartService.CreateCartAsync(result.Id);
            return Ok(result);
        }
        return BadRequest("User not found");
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginRequest request)
    {
        var result = await userService.LoginAsync(request);
        return Ok(result);
    }
}