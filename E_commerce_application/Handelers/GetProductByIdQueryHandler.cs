using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers;

public class GetProductByIdQueryHandler(IProductRepository productRepository)  : IRequestHandler<GetIdProductQuery, Result<ProductInDetailsDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    

    public async Task<Result<ProductInDetailsDto>> Handle(
        GetIdProductQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (product is null)
        {
            return Result<ProductInDetailsDto>.Failure(
                ProductError.NotFound);
        }

        var productDto = new ProductInDetailsDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            product.ImageUrl,
            product.ProductBrand.Name,
            product.ProductCategory.Name
        );

        return Result<ProductInDetailsDto>.Success(productDto);
    }
}