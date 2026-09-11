using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.CartHandlers;

public class GetCartHandler(
    IRedisService redisService,
    ICurrentUserService currentUserService
) : IRequestHandler<GetCartQuery, Result<CartResult<CartItemDto>>>
{
    public async Task<Result<CartResult<CartItemDto>>> Handle(
        GetCartQuery request,
        CancellationToken cancellationToken)
    {
        var buyerId = currentUserService.UserId;

        var key = $"cart:{buyerId}";

        var cart = await redisService.GetAsync<Cart>(key);

        if (cart is null)
            return Result<CartResult<CartItemDto>>.Failure(
                CartError.InvalidBuyerId);

        var cartDto = new CartResult<CartItemDto>(
            cart.BuyerId,
            cart.Items
                .Select(item => new CartItemDto(
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.UnitPrice,
                    item.Quantity,
                    item.LineTotal))
                .ToList(),
            cart.TotalItems,
            cart.SubTotal);

        return Result<CartResult<CartItemDto>>.Success(cartDto);
    }
}