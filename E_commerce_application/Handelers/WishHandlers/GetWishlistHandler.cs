using System.Security.Claims;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_application.Queries;
using E_commerce_domain.Entities;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.WishHandlers;

public class GetWishlistHandler(
    IWishlistRepository wishlistRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<WishlistQuery, Result<IReadOnlyList<ProductDTO>>>
{
    public async Task<Result<IReadOnlyList<ProductDTO>>> Handle(
        WishlistQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is null)
            return Result<
        IReadOnlyList<ProductDTO>>.Failure(new Error(
                    "User.UnAuthorized",
                    "User is not authorized",
                    ErrorType.Unauthorized
                ));

        var wishlist = await wishlistRepository.GetByUserIdWithItemsAsync(userId.Value, cancellationToken);
            

        if (wishlist is null)
        {
            return Result<IReadOnlyList<ProductDTO>>.Success(new List<ProductDTO>());
        }


        var response = wishlist.WishItems
            .Select(x => new ProductDTO
            (
                x.Product.Id,
                x.Product.Name,
                x.Product.Price,
                x.Product.Description,
                x.Product.ImageUrl
            ))
            .ToList();
        

        return Result<IReadOnlyList<ProductDTO>>.Success(response);
    }
}