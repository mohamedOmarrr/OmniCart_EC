namespace E_commerce_infrastructure.Identities;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Customer = "Customer";

    public static readonly IReadOnlyList<string> All =
    [
        SuperAdmin,
        Admin,
        Customer
    ];
}
