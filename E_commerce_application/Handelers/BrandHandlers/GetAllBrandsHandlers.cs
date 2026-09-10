using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.BrandHandlers;

public class GetAllBrandsHandlers( IRepository<Brand> repository)
    : IRequestHandler<BrandQuery, Result<IReadOnlyList<BrandDto>>>
{
    public async Task<Result<IReadOnlyList<BrandDto>>> Handle(
        BrandQuery request,
        CancellationToken cancellationToken)
    {

        var brands = await repository.GetAllAsync(cancellationToken);

        var brandDto = brands.Select(b => new BrandDto(
            b.Id,
            b.Name
        )).ToList();
        

        return Result<IReadOnlyList<BrandDto>>.Success(brandDto);
    }
}