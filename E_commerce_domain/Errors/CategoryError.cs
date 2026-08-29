using E_commerce_domain.shared;

namespace E_commerce_domain.Errors;

public static class CategoryError
{
    public static readonly Error NotFound =
        Error.NotFound(
            "ProductType.NotFound",
            "Type was not found.");

    public static readonly Error InvalidId =
        Error.Validation(
            "ProductType.InvalidId",
            "Type id is required.");

    public static readonly Error InvalidName =
        Error.Validation(
            "ProductType.InvalidName",
            "Type name is required.");
}