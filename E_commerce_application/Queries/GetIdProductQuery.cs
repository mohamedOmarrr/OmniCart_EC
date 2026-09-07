using E_commerce_application.DTOs;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Queries;

public record GetIdProductQuery(Guid Id): IRequest<Result<ProductInDetailsDto>>;