using E_commerce_domain.shared;

namespace E_commerce_domain.Errors;

public static class BrandError
{
    public static readonly Error NotFound =
        Error.NotFound(
            "ProductBrand.NotFound",
            "Brand was not found.");

    public static readonly Error InvalidId =
        Error.Validation(
            "ProductBrand.InvalidId",
            "Brand id is required.");

    public static readonly Error InvalidName =
        Error.Validation(
            "ProductBrand.InvalidName",
            "Brand name is required.");
}