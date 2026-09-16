using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Response_Patterns;
using E_commerce_application.Response_Patterns.Payment;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using E_commerce_infrastructure.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_application.Handelers.OrderHandlers;

 public sealed class CreateOrderCommandHandler(
    ICurrentUserService currentUserService,
    IRedisService redisService,
    IRepository<DeliveryMethod> deliveryMethodRepository,
    IRepository<Order> orderRepository,
    IPaymentService paymentService,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<CreateOrderCommand, Result<BaseOrderResponse>>
{
    public async Task<Result<BaseOrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
       
        var buyerId = currentUserService.UserId;

        if (buyerId is null)
            return Result<BaseOrderResponse>.Failure(
                Error.Failure(
                    "User.NotFound",
                    "Failed to Find This User"));

        
        var key = $"cart:{buyerId}";

        var cart = await redisService.GetAsync<Cart>(key);

        if (cart?.Items is null)
            return Result<BaseOrderResponse>.Failure(
                OrderError.EmptyBasket);


        
        var deliveryMethod =
            await deliveryMethodRepository.GetByIdAsync(
                request.DeliveryMethodId,
                cancellationToken);

        if (deliveryMethod is null)
            return Result<BaseOrderResponse>.Failure(
                DeliveryMethodError.InvalidId);

        if (!deliveryMethod.IsAvailable)
            return Result<BaseOrderResponse>.Failure(
                DeliveryMethodError.NotFound);

        
        var cartItems = cart.Items
            .Select(item => (
                ProductId: item.ProductId,
                ProductName: item.ProductName,
                PictureUrl: item.PictureUrl,
                UnitPrice: item.UnitPrice,
                Quantity: item.Quantity
            ))
            .ToList();
        
        
        var orderResult = Order.Create(
            id: Guid.NewGuid(),
            userId: buyerId.Value,
            paymentMethod: request.PaymentMethod,
            deliveryMethod: deliveryMethod,
            orderShippingDetails: OrderShippingDetails.Create(
                request.BuyerDetails.BuyerName,
                request.BuyerDetails.MainPhoneNumber,
                request.BuyerDetails.StepPhoneNumber,
                request.BuyerDetails.Address),
            cartItems: cartItems);

        if (orderResult.IsFailure)
            return Result<BaseOrderResponse>.Failure(
                OrderError.InvalidPaymentTransaction);

        var order = orderResult.Value;

        
        if (request.PaymentMethod == PaymentMethod.CashOnDelivery)
        {
            orderRepository.Add(order);

            await orderRepository.SaveChangesAsync(
                cancellationToken);

            var receipt 
                = new OrderReceiptDto(
                order.Items.Select(item => new OrderItemDto(
                    item.OrderId,
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.UnitPrice,
                    item.Quantity,
                    item.LineTotal)).ToList(),
                order.SubTotal,
                order.ShippingCost,
                order.Total, 
                order.Status.ToString(),
                order.DeliveryMethodName,
                order.DeliveryMethodEstimatedTime,
                order.PaymentMethod == PaymentMethod.CashOnDelivery
                    ? "cash-on-delivery"
                    : "card",
                order.ShippingRecipientFullname,
                order.ShippingMainPhoneNumber,
                order.ShippingStepPhoneNumber,
                order.ShippingAddress
            );
            
                return Result<BaseOrderResponse>.Success(
                new CashOnDeliveryResponse(
                    order.Id,
                    receipt));
        }

        
        var user = await userManager.FindByIdAsync(
            buyerId.Value.ToString());

        if (user is null)
            return Result<BaseOrderResponse>.Failure(
            Error.Failure(
                "User.NotFound",
                "Failed to Find This User"));

        
        var (firstName, lastName) =
            SplitName(request.BuyerDetails.BuyerName);

        
        var paymentResult =
            await paymentService.CreatePaymentAsync(
                order.Id,
                order.Total,
                order.ShippingCost,
                order.Items,
                firstName,
                lastName,
                user.Email!,
                cancellationToken);

        if (!paymentResult.IsSuccess)
        {
            return Result<BaseOrderResponse>.Failure(
                OrderError.InvalidPaymentTransaction);
        }

        var payment = paymentResult.Response!;

        
        var attachResult =
            order.AttachPayment(
                payment.IntentionOrderId.ToString());

        if (attachResult.IsFailure)
            return Result<BaseOrderResponse>.Failure(
                attachResult.Error);

        
        orderRepository.Add(
            order);

        await orderRepository.SaveChangesAsync(
            cancellationToken);

        
        return Result<BaseOrderResponse>.Success(
            new CardPaymentResponse(
                order.Id,
                payment.IntentionOrderId.ToString(),
                payment.ClientSecret));
    }

    private static (string FirstName, string LastName) SplitName(
        string fullName)
    {
        var parts = fullName
            .Trim()
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
            return (parts[0], parts[0]);

        return (
            parts[0],
            string.Join(' ', parts.Skip(1)));
    }
}