using E_commerce_domain.shared;

namespace E_commerce_domain.Errors;

public static class CartError
{
     public static readonly Error GuestBuyerIdRequired =
        Error.Validation(
            "Basket.GuestBuyerIdRequired",
            "Guest shoppers must send the X-Buyer-Id header with a client-generated GUID.");

    public static readonly Error AuthenticatedBuyerIdMissing =
        Error.Validation(
            "Basket.AuthenticatedBuyerIdMissing",
            "The user id claim is missing or invalid in the authentication token.");
    
    public static readonly Error ConflictInCart =
        Error.Validation(
            "Product.IsConflict",
            "This product is already in a cart.");

    public static readonly Error InvalidBuyerId =
        Error.Validation(
            "Basket.InvalidBuyerId",
            "A valid buyer id is required.");

    public static readonly Error InvalidProductId =
        Error.Validation(
            "Basket.InvalidProductId",
            "Product id is required.");

    public static readonly Error InvalidProductName =
        Error.Validation(
            "Basket.InvalidProductName",
            "Product name is required.");

    public static readonly Error InvalidPictureUrl =
        Error.Validation(
            "Basket.InvalidPictureUrl",
            "Product picture URL is required.");

    public static readonly Error InvalidUnitPrice =
        Error.Validation(
            "Basket.InvalidUnitPrice",
            "Unit price must be greater than zero.");

    public static readonly Error InvalidQuantity =
        Error.Validation(
            "Basket.InvalidQuantity",
            $"Quantity must be between {Entities.CartItem.MinQuantity} and {Entities.CartItem.MaxQuantity}.");

    public static readonly Error InvalidQuantityIncrement =
        Error.Validation(
            "Basket.InvalidQuantityIncrement",
            "Quantity increment must be greater than zero.");

    public static readonly Error QuantityTooHigh =
        Error.Validation(
            "Basket.QuantityTooHigh",
            $"Total quantity for a product cannot exceed {Entities.CartItem.MaxQuantity}.");

    public static readonly Error ItemNotFound =
        Error.NotFound(
            "Basket.ItemNotFound",
            "The product was not found in the basket.");

    public static readonly Error AnonymousBasketNotFound =
        Error.NotFound(
            "Basket.AnonymousBasketNotFound",
            "The anonymous basket to merge was not found or is already empty.");

    public static readonly Error CannotMergeSameBuyer =
        Error.Validation(
            "Basket.CannotMergeSameBuyer",
            "Cannot merge a basket into itself.");

    public static readonly Error AnonymousBuyerRequired =
        Error.Validation(
            "Basket.AnonymousBuyerRequired",
            "Anonymous buyer id is required for merge.");
}