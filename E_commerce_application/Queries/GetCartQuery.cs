using E_commerce_application.DTOs;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Queries;

public record GetCartQuery(): IRequest<Result<CartResult<CartItemDto>>>;