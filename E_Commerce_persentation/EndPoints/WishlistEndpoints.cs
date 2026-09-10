using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Queries;
using E_Commerce_persentation.Extensions;
using E_commerce_persentation.ResponseShapes;
using MediatR;

namespace E_Commerce_persentation.EndPoints;

public static class WishlistEndpoints
{
    public static IEndpointRouteBuilder MapWishlistEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/wish")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapGet("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new WishlistQuery();
                var result = await sender.Send(query, ct);

                return result.GetListedResults("Your Fav Products in Wishlist retrieved successfully");
            })
            .WithSummary("Gets all Products in Wishlist")
            .WithDescription("Returns a list of Products that user Sored in Wishlist")
            .Produces<ApiResponse<IReadOnlyList<ProductDTO>>>(StatusCodes.Status200OK);
        
        
        
        group.MapPost("/", async (
                [AsParameters] WishlistCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
               
                
                var result = await sender.Send(command, ct);

                return result.CommandResult("Product Added to Wishlist successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Create Wishlist or Add Product to it")
            .WithDescription("Returns Product ID that you Added to Wishlist")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created);

        return endpoints;
    }
}