using E_commerce_application.Response_Patterns.Payment;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using E_Commerce_persentation.HttpRequests;
using MediatR;

namespace E_commerce_application.Commands;

public record CreateOrderCommand
    (
        BuyerDetailsRequest BuyerDetails,
        Guid DeliveryMethodId,
        PaymentMethod PaymentMethod
    ) : IRequest<Result<BaseOrderResponse>>;