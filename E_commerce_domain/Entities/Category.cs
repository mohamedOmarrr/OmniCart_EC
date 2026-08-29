using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = null!;

    public ICollection<Product> Products { get; private set; } = [];

    private Category()
    {
    }

    public static Result<Category> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result<Category>.Failure(CategoryError.InvalidId);

        if (string.IsNullOrWhiteSpace(name))
            return Result<Category>.Failure(CategoryError.InvalidName);

        var productType = new Category
        {
            Id = id,
            Name = name.Trim()
        };

        return Result<Category>.Success(productType);
    }
}