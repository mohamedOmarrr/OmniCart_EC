using E_commerce_application.Interfaces;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_infrastructure.Repos;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{


    public async Task<PagedResult<Product>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? searchType,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = searchType?.ToLower() switch
            {
                "name" =>
                    query.Where(p => p.Name.Contains(search)),

                "brand" =>
                    query.Where(p => p.ProductBrand.Name.Contains(search)),

                "category" =>
                    query.Where(p => p.ProductCategory.Name.Contains(search)),

                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(
            products,
            pageNumber,
            pageSize,
            totalCount,
            totalPages);

    }
}