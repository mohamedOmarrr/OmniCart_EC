using System.Security.Claims;
using E_commerce_application.Commands;
using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.WishHandlers;

public class AddToWishlistHandler(
    IWishlistRepository wishlistRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<WishlistCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        WishlistCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;


        if (userId is null)
            return Result<Guid>.Failure(new Error(
                "User.UnAuthorized",
                "User is not authorized",
                ErrorType.Unauthorized
            ));
            

        var wishlist = await wishlistRepository.GetByUserIdWithItemsAsync(
            userId.Value,
            cancellationToken);

        if (wishlist is null)
        {
            wishlist = new Wishlist
            {
                UserId = userId.Value
            };
            
            wishlistRepository.Add(wishlist);
        }
            

        var exists = wishlist.WishItems
            .Any(x => x.ProductId == request.ProductId);

        if (exists)
            return Result<Guid>.Failure(new Error(
                "Product.Conflicted",
                "Product is already exists",
                ErrorType.Conflict
            ));

        wishlist.addWishItem(request.ProductId);

        await wishlistRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(request.ProductId);
    }
}