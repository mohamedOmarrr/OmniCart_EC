using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.CartHandlers;

public class RemoveCartItemHandler(
    IRedisService redisService,
    ICurrentUserService currentUserService
) : IRequestHandler<DeleteActionOnCartCommand, Result>
{
    public async Task<Result> Handle(
        DeleteActionOnCartCommand request,
        CancellationToken cancellationToken)
    {
        var buyerId = currentUserService.UserId;

        var key = $"cart:{buyerId}";

        var cart = await redisService.GetAsync<Cart>(key);

        if (cart is null)
            return Result.Failure(
                CartError.InvalidBuyerId);

        if (request.Id is not null)
            cart.RemoveItem(request.Id.Value);
     
        
        await redisService.SetAsync(key, cart);

        return Result.Success();
    }
}