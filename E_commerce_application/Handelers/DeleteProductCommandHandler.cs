using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers;

public class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IPhotoService photoService)
    : IRequestHandler<IdProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IPhotoService _photoService = photoService;

    public async Task<Result> Handle(
        IdProductCommand request,
        CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(
            request.Id,
            ct);

        if (product is null)
            return Result.Failure(ProductError.NotFound);

        var imageUrl = product.ImageUrl;

        _productRepository.Delete(product);

        await _productRepository.SaveChangesAsync(ct);

        await _photoService.DeleteAsync(imageUrl);

        return Result.Success();
    }
}