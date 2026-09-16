using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.OrderHandlers;

public class GetMyOrdersQueryHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService) : 
    IRequestHandler<GetMyOrdersQuery, Result<IReadOnlyList<MyOrdersDto>>>
{
   

    public async Task<Result<IReadOnlyList<MyOrdersDto>>> Handle(
        GetMyOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is null)
            return Result<IReadOnlyList<MyOrdersDto>>.Failure(
                OrderError.Unauthorized);

        var orders = await orderRepository
            .GetByUserIdAsync(userId.Value.ToString(), cancellationToken);

        var response = orders
            .Select(order => new MyOrdersDto(
                order.Id,
                order.Status,
                order.Total,
                order.PaymentMethod == PaymentMethod.Card? "Card" : "Cash On Delivery",
                order.DeliveryMethodName,
                order.CreatedAt))
            .ToList();

        return Result<IReadOnlyList<MyOrdersDto>>.Success(response);
    }
}