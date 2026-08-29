namespace E_commerce_domain.Entities;

public class WishItem : BaseEntity
{
    public Guid WishlistId { get; set; }
    public Wishlist Wishlist { get; set; } = default!;
    public Guid ProductId { get; set; }
}