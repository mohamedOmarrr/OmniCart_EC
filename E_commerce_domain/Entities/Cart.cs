using System.Text.Json.Serialization;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class Cart
{
    public Guid BuyerId { get; private set; }

    public List<CartItem> Items { get; private set; } = [];


    [JsonConstructor]
    private Cart(Guid buyerId, List<CartItem>? items)
    {
        BuyerId = buyerId;
        Items = items ?? [];
    }

    private Cart(Guid buyerId)
    {
        BuyerId = buyerId;
        Items = [];
    }

    public static Result<Cart> CreateEmpty(Guid buyerId)
    {
        if (buyerId == Guid.Empty)
            return Result<Cart>.Failure(CartError.InvalidBuyerId);

        return new Cart(buyerId);
    }

    public int TotalItems => Items.Sum(item => item.Quantity);

    public decimal SubTotal => Items.Sum(item => item.LineTotal);



    public Result AddItem(Guid productId, string productName, string pictureUrl, 
        decimal unitPrice, int quantity)
    {
        var existingItem = Items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is not null)
            return existingItem.IncreaseQuantity(quantity);


        var createResult = CartItem.Create(productId, productName, pictureUrl, unitPrice, quantity);

        if (createResult.IsFailure)
            return Result.Failure(createResult.Error);

        Items.Add(createResult.Value);

        return Result.Success();
    }

    public Result RemoveItem(Guid productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
            return Result.Failure(CartError.ItemNotFound);

        Items.Remove(item);

        return Result.Success();
    }

    public Result UpdateItemQuantity(Guid productId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
            return Result.Failure(CartError.ItemNotFound);


        return item.SetQuantity(quantity);
    }

    public void Clear() => Items.Clear();


    public Result MergeFrom(Cart other)
    {
        if (other.BuyerId == BuyerId)
            return Result.Failure(CartError.CannotMergeSameBuyer);

        foreach (var item in other.Items)
        {
            var mergeResult = AddItem(item.ProductId, item.ProductName, item.PictureUrl,
                item.UnitPrice, item.Quantity);

            if (mergeResult.IsFailure)
                return mergeResult;
        }

        return Result.Success();
    }
}