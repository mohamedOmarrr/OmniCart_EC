using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class Brand : BaseEntity
{
    public string Name { get; private set; } = null!;

    public ICollection<Product> Products { get; private set; } = [];

    private Brand()
    {
    }

    public static Result<Brand> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result<Brand>.Failure(BrandError.InvalidId);

        if (string.IsNullOrWhiteSpace(name))
            return Result<Brand>.Failure(BrandError.InvalidName);

        var productBrand = new Brand
        {
            Id = id,
            Name = name.Trim()
        };

        return Result<Brand>.Success(productBrand);
    }
}