namespace EasyOnlineStore.Domain.Enums;

public enum UserRole
{
    Admin = 0,
    Seller = 1,
    Customer = 2,
    Developer = 3
}

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Seller = "Seller";
    public const string Customer = "Customer";
    public const string Developer = "Developer";
}