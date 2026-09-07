using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers;

public class GetPagedProductsQueryHandler(IProductRepository productRepository) : IRequestHandler<
    ProductPagedQuery,
    Result<PagedResult<ProductDTO>>>
{
    private readonly IProductRepository _productRepository = productRepository;
    
    
    public async Task<Result<PagedResult<ProductDTO>>> Handle(
        ProductPagedQuery request,
        CancellationToken ct)
    {
        var result = await _productRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.TypeOfSearch,
            ct);

        var products = result.Items
            .Select(p => new ProductDTO(
                p.Id,
                p.Name,
                p.Price,
                p.Description, 
                p.ImageUrl
            ))
            .ToList();

        var pagedResults = new PagedResult<ProductDTO>(
            products,
            result.PageNumber,
            result.PageSize,
            result.TotalCount,
            result.TotalPages);

        return Result<PagedResult<ProductDTO>>.Success(pagedResults);
    }
}
