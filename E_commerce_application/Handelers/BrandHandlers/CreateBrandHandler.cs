using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.BrandHandlers;

public class CreateBrandHandler(
    IRepository<Brand> repository)
    : IRequestHandler<NamedBrandCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        NamedBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brandResult = Brand.Create(
            Guid.NewGuid(),
            request.BrandName
        );

        if (brandResult.IsFailure)
        {
            return Result<Guid>.Failure(brandResult.Error);
        }
      
        var brand =  brandResult.Value;

        repository.Add(brand);

        await repository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(brand.Id);
    }
}