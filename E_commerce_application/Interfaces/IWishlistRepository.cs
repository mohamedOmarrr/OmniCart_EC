using E_commerce_domain.Entities;

namespace E_commerce_application.Interfaces;

public interface IWishlistRepository : IRepository<Wishlist>
{
    Task<Wishlist?> GetByUserIdWithItemsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}