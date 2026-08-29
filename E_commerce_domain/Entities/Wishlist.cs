namespace E_commerce_domain.Entities;

public class Wishlist : BaseEntity
{
    public string UserId { get; set; } = null!;

    public ICollection<WishItem> WishItems { get; set; } = new List<WishItem>();
}