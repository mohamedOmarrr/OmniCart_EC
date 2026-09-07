using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class Product :BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 1000;
    public const int MaxPictureUrlLength = 500;

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    
    public string ImageUrl { get; private set; } = null!;
    public decimal Price { get; private set; }

    public Guid ProductBrandId { get; private set; }
    public Brand ProductBrand { get; private set; } = null!;

    public Guid ProductCategoryId { get; private set; }
    public Category ProductCategory { get; private set; } = null!;

    private Product()
    {
    }

    public static Result<Product> Create(
        Guid id,
        string name,
        string description,
        string imageUrl,
        decimal price,
        Guid productBrandId,
        Guid productCategoryId)
    {
        if (id == Guid.Empty)
            return Result<Product>.Failure(ProductError.InvalidId);

        if (string.IsNullOrWhiteSpace(name))
            return Result<Product>.Failure(ProductError.InvalidName);

        if (name.Length > MaxNameLength)
            return Result<Product>.Failure(ProductError.NameTooLong);

        if (string.IsNullOrWhiteSpace(description))
            return Result<Product>.Failure(ProductError.InvalidDescription);

        if (description.Length > MaxDescriptionLength)
            return Result<Product>.Failure(ProductError.DescriptionTooLong);

        if (string.IsNullOrWhiteSpace(imageUrl))
            return Result<Product>.Failure(ProductError.InvalidPictureUrl);

        if (imageUrl.Length > MaxPictureUrlLength)
            return Result<Product>.Failure(ProductError.PictureUrlTooLong);

        if (price <= 0)
            return Result<Product>.Failure(ProductError.InvalidPrice);

        if (productBrandId == Guid.Empty)
            return Result<Product>.Failure(ProductError.InvalidBrand);

        if (productCategoryId == Guid.Empty)
            return Result<Product>.Failure(ProductError.InvalidType);

        var product = new Product
        {
            Id = id,
            Name = name.Trim(),
            Description = description.Trim(),
            ImageUrl = imageUrl.Trim(),
            Price = price,
            ProductBrandId = productBrandId,
            ProductCategoryId = productCategoryId
        };

        return Result<Product>.Success(product);
    }

    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(ProductError.InvalidName);

        if (name.Length > MaxNameLength)
            return Result.Failure(ProductError.NameTooLong);

        Name = name.Trim();

        return Result.Success();
    }

    public Result ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(ProductError.InvalidDescription);

        if (description.Length > MaxDescriptionLength)
            return Result.Failure(ProductError.DescriptionTooLong);

        Description = description.Trim();

        return Result.Success();
    }

    public Result ChangePictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result.Failure(ProductError.InvalidPictureUrl);

        if (ImageUrl.Length > MaxPictureUrlLength)
            return Result.Failure(ProductError.PictureUrlTooLong);

        ImageUrl = pictureUrl.Trim();

        return Result.Success();
    }

    public Result ChangePrice(decimal price)
    {
        if (price <= 0)
            return Result.Failure(ProductError.InvalidPrice);

        Price = price;

        return Result.Success();
    }

    public Result ChangeBrand(Guid productBrandId)
    {
        if (productBrandId == Guid.Empty)
            return Result.Failure(ProductError.InvalidBrand);

        ProductBrandId = productBrandId;

        return Result.Success();
    }

    public Result ChangeCategory(Guid productCategoryId)
    {
        if (productCategoryId == Guid.Empty)
            return Result.Failure(ProductError.InvalidType);

        ProductCategoryId = productCategoryId;

        return Result.Success();
    }
}