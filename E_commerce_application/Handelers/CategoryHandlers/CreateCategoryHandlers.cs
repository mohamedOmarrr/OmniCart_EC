using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.CategoryHandlers;

public class CreateCategoryHandlers(
    IRepository<Category> repository)
    : IRequestHandler<NamedCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        NamedCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryResult = Category.Create(
            Guid.NewGuid(),
            request.CategoryName
        );

        if (categoryResult.IsFailure)
        {
            return Result<Guid>.Failure(categoryResult.Error);
        }
      
        var category =  categoryResult.Value;

        repository.Add(category);

        await repository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(category.Id);
    }
}