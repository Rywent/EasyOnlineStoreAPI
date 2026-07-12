namespace EasyOnlineStore.Domain.Enums;

public enum Permission
{
    None = 0,
    Read = 1,
    Create = 2,
    Update = 4,
    Delete = 8,
    All = Read | Create | Update | Delete
}