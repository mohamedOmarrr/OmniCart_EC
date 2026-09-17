using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;

namespace E_commerce_application.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedResult<Product>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? searchType,
        CancellationToken cancellationToken = default);
    
    Task<Product?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
