using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IRepository<Brand> brandRepository,
    IRepository<Category> categoryRepository,
    IPhotoService photoService)  : IRequestHandler<ProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IRepository<Brand> _brandRepository = brandRepository;
    private readonly IRepository<Category> _categoryRepository = categoryRepository;
    private readonly IPhotoService _photoService = photoService;

    public async Task<Result<Guid>> Handle(
        ProductCommand request,
        CancellationToken cancellationToken)
    {
        // Upload image
        var imageUrl = await _photoService.UploadAsync(
            request.StreamImage,
            request.FileName,
            $"{request.CategoryName}");
        
        var category = await _categoryRepository.FirstOrDefaultAsync(
            x => x.Name == request.CategoryName,
            cancellationToken);

        if (category is null)
        {
            return Result<Guid>.Failure(
                CategoryError.NotFound);
        }
        
        
        var brand = await _brandRepository.FirstOrDefaultAsync(
            x => x.Name == request.BrandName,
            cancellationToken);

        if (brand is null)
        {
            return Result<Guid>.Failure(
                BrandError.NotFound);
        }

       
        var productResult = Product.Create(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            imageUrl,
            request.Price,
            category.Id,
            brand.Id);
        
        if (productResult.IsFailure)
            return Result<Guid>.Failure(productResult.Error);

        var product = productResult.Value;

        
        _productRepository.Add(product);
        
        await _productRepository.SaveChangesAsync(
            cancellationToken);
        
        return Result<Guid>.Success(product.Id);
    }

}