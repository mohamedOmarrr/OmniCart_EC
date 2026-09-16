using E_commerce_application.DTOs;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Queries;

public record DeliveryOrderQuery(): IRequest<Result<IReadOnlyList<DeliveryDto>>>;