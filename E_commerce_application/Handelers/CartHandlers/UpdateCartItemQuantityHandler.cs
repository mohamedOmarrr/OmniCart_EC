using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.CartHandlers;

public class UpdateCartItemQuantityHandler(
    IRedisService redisService,
    ICurrentUserService currentUserService
) : IRequestHandler<
    CartQuantityCommand,
    Result<CartResult<CartItemDto>>>
{
    public async Task<Result<CartResult<CartItemDto>>> Handle(
        CartQuantityCommand request,
        CancellationToken cancellationToken)
    {
        var buyerId = currentUserService.UserId;

        var key = $"cart:{buyerId}";

        var cart = await redisService.GetAsync<Cart>(key);

        if (cart is null)
            return Result<CartResult<CartItemDto>>.Failure(
                CartError.InvalidBuyerId);

        var result = cart.UpdateItemQuantity(
            request.ProductId,
            request.Quantity);

        if (result.IsFailure)
            return Result<CartResult<CartItemDto>>.Failure(
                CartError.ItemNotFound);

        await redisService.SetAsync(key, cart);

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