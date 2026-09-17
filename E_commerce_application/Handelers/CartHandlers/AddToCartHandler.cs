using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.Errors;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.CartHandlers;

public class AddToCartHandler(
    IRepository<Product> productRepository,
    IRedisService redisService,
    ICurrentUserService currentUserService
) : IRequestHandler<AddToCartCommand, Result<CartItemDto>>
{
    public async Task<Result<CartItemDto>> Handle(
        AddToCartCommand request,
        CancellationToken cancellationToken)
    {
        var buyerId = currentUserService.UserId;


        if (buyerId is null)
        {
            return Result<CartItemDto>.Failure(CartError.InvalidBuyerId);
        }

        var product = await productRepository.GetByIdAsync(
            request.ProductId,
            cancellationToken);
       
        if (product is null)
            return Result<CartItemDto>.Failure(
                ProductError.NotFound);

        var key = $"cart:{buyerId}";

        var cart = await redisService.GetAsync<Cart>(key);

        if (cart is null)
        {
            var emptyCart = Cart.CreateEmpty(buyerId.Value);
            
            cart = emptyCart.Value;
        }
        
        var existingItem = cart.Items
            .FirstOrDefault(x => x.ProductId == request.ProductId);

        if (existingItem is not null)
        {
            return Result<CartItemDto>.Failure(
                CartError.ConflictInCart);
        }

        var addCartItem = cart.AddItem(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Price,
            1);

        if (addCartItem.IsFailure)
            return Result<CartItemDto>.Failure(
                CartError.ItemNotFound
                );

        await redisService.SetAsync(key, cart);

        var item = cart.Items.First(
            x => x.ProductId == request.ProductId);

        var cartItemDto = new CartItemDto(
                item.ProductId,
                item.ProductName,
                item.PictureUrl,
                item.UnitPrice,
                item.Quantity,
                item.LineTotal);
        
        
        return Result<CartItemDto>.Success(cartItemDto);
    }
}