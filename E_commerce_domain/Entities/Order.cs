using E_commerce_domain.Errors;
using E_commerce_domain.shared;

namespace E_commerce_domain.Entities;

public class Order : BaseEntity
{
     public const int MaxNameLength = 100;
    public const int MaxPhoneLength = 32;
    public const int MaxCountryLength = 100;
    public const int MaxCityLength = 100;
    public const int MaxStreetLength = 200;
    public const int MaxPostalCodeLength = 20;
    public const int MaxDeliveryMethodNameLength = 100;
    public const int MaxDeliveryTimeLength = 100;

    private readonly List<OrderItem> _items = [];

    private Order()
    {
    }

    public Guid UserId { get; private set; }
    public OrderStatus Status { get; private set; }
    
    public PaymentMethod PaymentMethod { get; private set; }

    public Guid DeliveryMethodId { get; private set; }
    public string DeliveryMethodName { get; private set; } = null!;

    public string DeliveryMethodEstimatedTime { get; private set; } = null!;

    public string ShippingRecipientFullname { get; private set; } = null!;
    public string ShippingMainPhoneNumber { get; private set; } = null!;
    public string ShippingStepPhoneNumber { get; private set; } = null!;
    public string ShippingAddress { get; private set; } = null!;
    
    public decimal SubTotal { get; private set; }
    public decimal ShippingCost { get; private set; }
    public decimal Total { get; private set; }

    public string? PaymentTransactionId { get; private set; }
    public DateTimeOffset? PaidAtUtc { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public static Result<Order> Create(
        Guid id,
        Guid userId,
        PaymentMethod paymentMethod,
        DeliveryMethod? deliveryMethod,
        OrderShippingDetails? orderShippingDetails,
        IReadOnlyList<(Guid ProductId, string ProductName, string PictureUrl, decimal UnitPrice, int Quantity)>? cartItems)
    {
        if (id == Guid.Empty)
            return Result<Order>.Failure(OrderError.InvalidId);

        if (userId == Guid.Empty)
            return Result<Order>.Failure(OrderError.InvalidUserId);

        if (deliveryMethod is null)
            return Result<Order>.Failure(OrderError.DeliveryMethodRequired);

        if (!deliveryMethod.IsAvailable)
            return Result<Order>.Failure(OrderError.DeliveryMethodUnavailable);

        if (orderShippingDetails is null)
            return Result<Order>.Failure(OrderError.ShippingAddressRequired);
        

        if (cartItems is null || cartItems.Count == 0)
            return Result<Order>.Failure(OrderError.EmptyBasket);

        var order = new Order
        {
            Id = id,
            UserId = userId,
            Status = OrderStatus.Pending,
            PaymentMethod = paymentMethod,
            DeliveryMethodId = deliveryMethod.Id,
            DeliveryMethodName = deliveryMethod.Name,
            DeliveryMethodEstimatedTime = deliveryMethod.EstimatedDeliveryTime,
            ShippingRecipientFullname = orderShippingDetails.BuyerName,
            ShippingMainPhoneNumber = orderShippingDetails.MainPhoneNumber,
            ShippingStepPhoneNumber = orderShippingDetails.StepPhoneNumber,
            ShippingAddress = orderShippingDetails.Address,
            ShippingCost = deliveryMethod.Price,
            CreatedAt = DateTimeOffset.UtcNow
        };

        foreach (var line in cartItems)
        {
            var itemResult = OrderItem.Create(
                Guid.NewGuid(),
                line.ProductId,
                line.ProductName,
                line.PictureUrl,
                line.UnitPrice,
                line.Quantity
            );

            if (itemResult.IsFailure)
                return Result<Order>.Failure(itemResult.Error);

            var item = itemResult.Value;
            item.AssignOrder(id);
            order._items.Add(item);
        }

        order.SubTotal = order._items.Sum(i => i.LineTotal);
        order.Total = order.SubTotal + order.ShippingCost;

        return Result<Order>.Success(order);
    }

    public Result Cancel()
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderError.CannotCancel);

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result AttachPayment(string paymentTransactionId)
    {
        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderError.InvalidPaymentState);

        if (string.IsNullOrWhiteSpace(paymentTransactionId))
            return Result.Failure(OrderError.InvalidPaymentTransaction);

        PaymentTransactionId = paymentTransactionId.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
        return Result.Success();
    }

    public Result MarkAsPaid(string paymentTransactionId)
    {
        if (Status == OrderStatus.Cancelled)
            return Result.Failure(OrderError.CannotPayCancelled);

        // Idempotent: already paid with same intent
        if (Status == OrderStatus.Processing
            && PaymentTransactionId == paymentTransactionId
            && PaidAtUtc is not null)
            return Result.Success();

        if (Status != OrderStatus.Pending)
            return Result.Failure(OrderError.InvalidPaymentState);

        if (!string.IsNullOrWhiteSpace(PaymentTransactionId)
            && PaymentTransactionId != paymentTransactionId)
            return Result.Failure(OrderError.PaymentTransactionMismatch);

        PaymentTransactionId = paymentTransactionId;
        PaidAtUtc = DateTimeOffset.UtcNow;
        Status = OrderStatus.Processing;
        UpdatedAt = DateTimeOffset.UtcNow;
        return Result.Success();
    }
    
    public Result MarkAsShipped()
    {
        if (Status == OrderStatus.Cancelled)
            return Result.Failure(OrderError.PaymentFailed);

        if (Status != OrderStatus.Processing)
            return Result.Failure(OrderError.InvalidPaymentState);

        Status = OrderStatus.Shipped;
        UpdatedAt = DateTimeOffset.UtcNow;

        return Result.Success();
    }
    
    public Result MarkAsDelivered()
    {
        if (Status == OrderStatus.Cancelled)
            return Result.Failure(OrderError.PaymentFailed);

        if (Status != OrderStatus.Shipped)
            return Result.Failure(OrderError.InvalidPaymentState);

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTimeOffset.UtcNow;

        return Result.Success();
    }
}