using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.OrderHandlers;

public class GetAllOrdersTOAdminHandler(IOrderRepository orderRepository, ICurrentUserService  currentUserService)
    : IRequestHandler<
        GetAllOrdersToAdminQuery,
        Result<IReadOnlyList<AllOrdersDto>>>
{
 

    public async Task<Result<IReadOnlyList<AllOrdersDto>>> Handle(
        GetAllOrdersToAdminQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        
        var orders = await orderRepository.GetByOrderStatusAsync(
            request.OrderStatus,
            cancellationToken);

        if (orders is null)
        {
            return Result<IReadOnlyList<AllOrdersDto>>.Failure(OrderError.NotFound);
        }

        var response = orders
            .Select(order => new AllOrdersDto(
                order.Id,
                order.UserId,
                order.Status.ToString(),
                order.PaymentMethod == PaymentMethod.CashOnDelivery
                    ? "cash-on-delivery"
                    : "card",
                order.DeliveryMethodName,
                order.ShippingRecipientFullname,
                order.ShippingMainPhoneNumber,
                order.Total,
                order.CreatedAt
            ))
            .ToList();

        return Result<IReadOnlyList<AllOrdersDto>>.Success(
            response);
    }
}