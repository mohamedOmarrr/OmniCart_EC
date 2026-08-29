using System.Text.Json.Serialization;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class CartItem :BaseEntity
{
     public const int MinQuantity = 1;
    public const int MaxQuantity = 99;


    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public string PictureUrl { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    [JsonConstructor]
    private CartItem(Guid productId, string productName, string pictureUrl, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        PictureUrl = pictureUrl;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static Result<CartItem> Create(Guid productId, string productName, 
        string pictureUrl, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            return Result<CartItem>.Failure(CartError.InvalidProductId);

        if (string.IsNullOrWhiteSpace(productName))
            return Result<CartItem>.Failure(CartError.InvalidProductName);

        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result<CartItem>.Failure(CartError.InvalidPictureUrl);

        if (unitPrice <= 0)
            return Result<CartItem>.Failure(CartError.InvalidUnitPrice);

        if (quantity is < MinQuantity or > MaxQuantity)
            return Result<CartItem>.Failure(CartError.InvalidQuantity);

        return Result<CartItem>.Success(
            new CartItem(
                productId,
                productName.Trim(),
                pictureUrl.Trim(),
                unitPrice,
                quantity));
    }

    public decimal LineTotal => UnitPrice * Quantity;


    public Result IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            return Result.Failure(CartError.InvalidQuantityIncrement);

        var newQuantity = Quantity + amount;

        if (newQuantity > MaxQuantity)
            return Result.Failure(CartError.QuantityTooHigh);

        Quantity = newQuantity;

        return Result.Success();
    }


    public Result SetQuantity(int quantity)
    {
        if (quantity is < MinQuantity or > MaxQuantity)
            return Result.Failure(CartError.InvalidQuantity);

        Quantity = quantity;

        return Result.Success();
    }

    public Result UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            return Result.Failure(CartError.InvalidUnitPrice);

        UnitPrice = unitPrice;

        return Result.Success();
    }
}