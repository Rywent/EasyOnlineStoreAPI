using EasyOnlineStore.Application.DTOs.Requests.User;
using EasyOnlineStore.Application.DTOs.Responses.User;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[Authorize]
public class UsersController(IUserService userService) : BaseApiController
{
    #region AllUsers
    
    // GET /api/users/profile
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileResponse>> GetProfile(CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var profile = await userService.GetProfileAsync(userId, ct);
        return Ok(profile);
    }
    
    // PUT /api/users/profile
    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var updatedProfile = await userService.UpdateProfileAsync(userId, request, ct);
        return Ok(updatedProfile);
    }
    
    // POST /api/users/change-password
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] string newPassword, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        await userService.ChangePasswordAsync(userId, newPassword, ct);
        return Ok(new { Message = "Password changed successfully" });
    }
    
    #endregion
    
    #region Admin and Developer
    
    // GET: api/users/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id, CancellationToken ct = default)
    {
        var user = await userService.GetUserByIdAsync(id, ct);
        return Ok(user);
    }
    
    // GET: api/users?page=1&pageSize=10
    [HttpGet]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<UserResponse>>> GetByPage(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var users = await userService.GetByPageAsync(page, pageSize, ct);
        return Ok(users);
    }
    
    // DELETE: api/users/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken ct = default)
    {
        await userService.DeleteUserAsync(id, ct);
        return NoContent();
    }
    
    // POST: api/users/{id}/lock
    [HttpPost("{id:guid}/lock")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult> LockUser(Guid id, CancellationToken ct = default)
    {
        await userService.LockUserAsync(id, ct);
        return Ok(new { Message = "User locked successfully" });
    }
    
    // POST: api/users/{id}/unlock
    [HttpPost("{id:guid}/unlock")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult> UnlockUser(Guid id, CancellationToken ct = default)
    {
        await userService.UnlockUserAsync(id, ct);
        return Ok(new { Message = "User unlocked successfully" });
    }
    
    // PUT: api/users/{id}/role
    [HttpPut("{id:guid}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateUserRole(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken ct = default)
    {
        await userService.UpdateUserRolesAsync(id, request.Roles, ct);
        return Ok(new { Message = $"User roles updated to {string.Join(", ", request.Roles)} successfully" });
    }

    // GET: api/users/{id}/roles
    [HttpGet("{id:guid}/roles")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<string>>> GetUserRoles(Guid id, CancellationToken ct = default)
    {
        var roles = await userService.GetUserRolesAsync(id, ct);
        return Ok(roles);
    }
    
    #endregion
}