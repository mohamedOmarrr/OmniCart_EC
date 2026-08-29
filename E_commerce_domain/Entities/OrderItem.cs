using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class OrderItem : BaseEntity
{
    private OrderItem()
    {
    }
    
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string PictureUrl { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal LineTotal => UnitPrice * Quantity;

    internal static Result<OrderItem> Create(
        Guid id,
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        if (id == Guid.Empty)
            return Result<OrderItem>.Failure(OrderError.InvalidItemId);

        if (productId == Guid.Empty)
            return Result<OrderItem>.Failure(OrderError.InvalidProductId);
        
        if(pictureUrl == string.Empty)
            return Result<OrderItem>.Failure(OrderError.InvalidPictureUrl);

        if (unitPrice < 0)
            return Result<OrderItem>.Failure(OrderError.InvalidUnitPrice);

        if (string.IsNullOrWhiteSpace(productName))
            return Result<OrderItem>.Failure(OrderError.InvalidProductName);
        
        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result<OrderItem>.Failure(OrderError.InvalidProductName);

        if (quantity < 1)
            return Result<OrderItem>.Failure(OrderError.InvalidQuantity);

        return Result<OrderItem>.Success(new OrderItem
        {
            Id = id,
            ProductId =  productId,
            ProductName =  productName.Trim(),
            UnitPrice =  unitPrice,
            PictureUrl =   pictureUrl.Trim(),
            Quantity = quantity
        });
    }

    internal void AssignOrder(Guid orderId) => OrderId = orderId;
}