using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new BadHttpRequestException("Invalid user ID in token");
        
        return userId;
    }
}