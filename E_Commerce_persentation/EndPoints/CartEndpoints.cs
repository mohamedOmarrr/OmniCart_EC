using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Queries;
using E_commerce_application.Response_Patterns;
using E_Commerce_persentation.Extensions;
using E_commerce_persentation.ResponseShapes;
using MediatR;

namespace E_Commerce_persentation.EndPoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/cart")
            .WithTags("Cart")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapPost("/{id:guid}", async (
                [AsParameters] AddToCartCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("Product Added in Cart successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Add Products to Cart")
            .WithDescription("Returns a CartItem Dto that user Added to Cart Successfully")
            .Produces<ApiResponse<CartItemDto>>(StatusCodes.Status200OK);
        
        
        
        group.MapGet("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new GetCartQuery();
                
                var result = await sender.Send(query, ct);

                return result.GetCartedResults("CartItems Retrieved to Cart successfully");
            })
            .WithSummary("Get all CartItems ")
            .WithDescription("Returns List of CartItem DTOs that you Added to Cart")
            .Produces<ApiResponse<CartResult<CartItemDto>>>(StatusCodes.Status200OK);
        
        group.MapDelete("/item/{productId:guid}", async (
                [AsParameters] DeleteActionOnCartCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("CartItem Is Deleted Successfully", StatusCodes.Status204NoContent);
            })
            .WithSummary("Delete One CartItem")
            .WithDescription("does not Return any When CartItem Deleted")
            .Produces(StatusCodes.Status204NoContent);
        
        
        
        group.MapDelete("/clear", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new DeleteActionOnCartCommand(null);
                
                var result = await sender.Send(command, ct);

                return result.CommandResult("Cart Is Cleared Successfully", StatusCodes.Status204NoContent);
            })
            .WithSummary("Clear all CartItems")
            .WithDescription("does not Return any When Clear All CartItems in Cart")
            .Produces(StatusCodes.Status204NoContent);
        
        
        group.MapDelete("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new DeleteActionOnCartCommand(null);
                var result = await sender.Send(command, ct);

                return result.CommandResult("Cart Is Deleted", StatusCodes.Status204NoContent);
            })
            .WithSummary("Delete Cart")
            .WithDescription("does not Return any When Cart Deleted")
            .Produces(StatusCodes.Status204NoContent);
        
        
        group.MapPatch("/item/", async (
                CartQuantityCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("CartItems is Updated Successfully", StatusCodes.Status200OK);
            })
            .WithSummary("Update Quantity")
            .WithDescription("Update Quantity in Item and Returns List of CartItems with TotalItems and SubTotal")
            .Produces<ApiResponse<CartResult<CartItemDto>>>(StatusCodes.Status200OK
            );
        
        

        return endpoints;
    }
}