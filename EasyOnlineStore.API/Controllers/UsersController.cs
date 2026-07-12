using System.Security.Claims;
using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    
    #region AllUsers
    // GET /api/users/profile/
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileResponse>> GetProfile()
    {
        var userId = GetUserIdFromToken();
        var profile = await userService.GetProfileAsync(userId);
        
        return Ok(profile);
        
    }
    
    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetUserIdFromToken();
        var updatedProfile = await userService.UpdateProfileAsync(userId, request);
        return Ok(updatedProfile);
    }
    
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] string newPassword)
    {
        var userId = GetUserIdFromToken();
        await userService.ChangePasswordAsync(userId, newPassword);
        return Ok(new { Message = "Password changed successfully" });
    }
    
    #endregion
    
    #region Admin and Developer
    
    // GET: api/users/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(user);
    }
    
    // GET api/users/all
    [HttpGet("all")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<UserResponse>>> GetAll()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    
    // DELETE: api/users/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }
    
    // POST: api/users/{id}/lock
    [HttpPost("{id:guid}/lock")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult> LockUser(Guid id)
    {
        await userService.LockUserAsync(id);
        return Ok(new { Message = "User locked successfully" });
    }
    
    // POST: api/users/{id}/unlock
    [HttpPost("{id:guid}/unlock")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult> UnlockUser(Guid id)
    {
        await userService.UnlockUserAsync(id);
        return Ok(new { Message = "User unlocked successfully" });
    }
    
    // PUT: api/users/{id}/role
    [HttpPut("{id:guid}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateUserRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        await userService.UpdateUserRolesAsync(id, request.Roles);
        return Ok(new { Message = $"User roles updated to {string.Join(", ", request.Roles)} successfully" });
    }

    // GET: api/users/{id}/roles
    [HttpGet("{id:guid}/roles")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<string>>> GetUserRoles(Guid id)
    {
        var roles = await userService.GetUserRolesAsync(id);
        return Ok(roles);
    }
    
    #endregion
    
    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new BadHttpRequestException("Invalid user ID in token");
        
        return userId;
    }
}