using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.OrderHandlers;

public class GetAllDeliveriesHandler
    (IRepository<DeliveryMethod> deliveryMethodRepository)
    : IRequestHandler<DeliveryOrderQuery, Result<IReadOnlyList<DeliveryDto>>>
{
    public async Task<Result<IReadOnlyList<DeliveryDto>>> Handle(
        DeliveryOrderQuery request,
        CancellationToken cancellationToken)
    {
        var delivery = await deliveryMethodRepository.GetAllAsync(cancellationToken);

        if (delivery is null)
        {
            return Result<IReadOnlyList<DeliveryDto>>.Failure(
                DeliveryMethodError.NotFound);
        }
        
        var deliveryItems = delivery
            .Select(item =>
            {
                if (item.Description != null)
                    return new DeliveryDto(
                        item.Id,
                        item.Name,
                        item.Description,
                        item.Price,
                        item.EstimatedDeliveryTime,
                        item.IsAvailable,
                        item.DisplayOrder
                    );
                return null;
            })
            .ToList();
        
        return Result<IReadOnlyList<DeliveryDto>>.Success(deliveryItems);
    }
}