using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.CategoryHandlers;

public class GetAllCategoriesHandlers( IRepository<Category> repository)
    : IRequestHandler<EmptyCategoryQuery, Result<IReadOnlyList<CategoryDto>>>
{
    public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(
        EmptyCategoryQuery request,
        CancellationToken cancellationToken)
    {

        var category = await repository.GetAllAsync(cancellationToken);

        var categoryDto = category.Select(b => new CategoryDto(
            b.Id,
            b.Name
        )).ToList();
        

        return Result<IReadOnlyList<CategoryDto>>.Success(categoryDto);
    }
}