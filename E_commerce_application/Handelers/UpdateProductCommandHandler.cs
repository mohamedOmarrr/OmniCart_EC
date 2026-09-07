using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IPhotoService photoService)  : IRequestHandler<UpdateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IPhotoService _photoService = photoService;

    public async Task<Result<Guid>> Handle(
        UpdateProductCommand request,
        CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, ct);
        
        if (product is null)
            return Result<Guid>.Failure(ProductError.NotFound);

        if (request.ImageStream is not null)
        {
            var imageUrl = await _photoService.UploadAsync(
                request.ImageStream,
                request.FileName,
                $"{product.ProductCategory.Name}");
            
            var imageResult = product.ChangePictureUrl(imageUrl);
            if (imageResult.IsFailure)
                return Result<Guid>.Failure(imageResult.Error);
        }


        if (request.Name is not null)
        {
            var nameResult = product.Rename(request.Name);

            if (nameResult.IsFailure)
                return Result<Guid>.Failure(nameResult.Error);
        }

        if (request.Description is not null)
        {
            var descriptionResult =
                product.ChangeDescription(request.Description);

            if (descriptionResult.IsFailure)
                return Result<Guid>.Failure(descriptionResult.Error);
        }

        if (request.Price.HasValue)
        {
            var priceResult =
                product.ChangePrice(request.Price.Value);

            if (priceResult.IsFailure)
                return Result<Guid>.Failure(priceResult.Error);
        }

        
        _productRepository.Update(product);
        
        await _productRepository.SaveChangesAsync(ct);
        
        await _photoService.DeleteAsync(product.ImageUrl);
        
        return Result<Guid>.Success(product.Id);   
    }
    
}