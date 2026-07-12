namespace EasyOnlineStore.Application.DTOs.Requests.User;

public class UpdateRoleRequest
{
    public List<string> Roles { get; set; } = new();
}