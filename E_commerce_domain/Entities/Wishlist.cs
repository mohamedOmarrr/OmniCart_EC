namespace E_commerce_domain.Entities;

public class Wishlist : BaseEntity
{
    public Guid UserId { get; set; }

    public ICollection<WishItem> WishItems { get; set; } = new List<WishItem>();

    public void addWishItem(Guid ProductId)
    {
        var wishItem = new WishItem
        {
            WishlistId = UserId,
            ProductId = ProductId,
        };
        
        WishItems.Add(wishItem);
    }
}