using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_infrastructure.Repos;

public class WishlistRepository(AppDbContext context)
    : Repository<Wishlist>(context), IWishlistRepository
{
    public async Task<Wishlist?> GetByUserIdWithItemsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.WishItems)
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);
    }
}