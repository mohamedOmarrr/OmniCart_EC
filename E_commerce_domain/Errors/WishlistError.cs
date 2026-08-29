using E_commerce_domain.shared;

namespace E_commerce_domain.Errors;

public class WishlistError
{
    public static Error ProductNotFound(Guid productId)
        => Error.NotFound("wishlist.ProductNotFound", $"Product with id {productId} does not exist");
    
    public static Error ProductAlreadyExists(Guid productId)
        => Error.Conflict("wishlist.ProductAlreadyExists", $"Product with id {productId} already exist on wishlist");

}