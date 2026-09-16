using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.OrderHandlers;

public class GetOrderReceiptQueryHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService)
    : IRequestHandler<GetOrderReceiptQuery, Result<OrderReceiptDto>>
{
    
    public async Task<Result<OrderReceiptDto>> Handle(
        GetOrderReceiptQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is null)
            return Result<OrderReceiptDto>.Failure(
                OrderError.Unauthorized);

        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
    

        if (order is null)
            return Result<OrderReceiptDto>.Failure(
                OrderError.NotFound);

        var response = new OrderReceiptDto(
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

        return Result<OrderReceiptDto>.Success(response);
    }
}