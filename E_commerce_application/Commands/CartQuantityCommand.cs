using E_commerce_application.DTOs;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record CartQuantityCommand(Guid  ProductId, int Quantity) : IRequest<Result<CartResult<CartItemDto>>>;