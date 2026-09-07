using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Queries;
using E_commerce_application.Response_Patterns;
using E_Commerce_persentation.Extensions;
using E_Commerce_persentation.HttpRequests;
using E_commerce_persentation.ResponseShapes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_persentation.EndPoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/products")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));
        
        group.MapGet("/paged", async (
            [AsParameters] ProductPagedQuery query,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(query, ct);

            return result.GetPagedResults("Products retrieved successfully");
        })
        .WithSummary("Gets paginated products")
        .WithDescription("Returns a paginated list of products with filtering and sorting options")
        .Produces<ApiResponse<PagedResult<ProductDTO>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", async (
                [AsParameters] GetIdProductQuery query,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(query, ct);

                return result.GetSpecificItem("Product id retrieved successfully");
            })
            .WithSummary("Gets product by ID")
            .WithDescription("Returns product information")
            .Produces<ApiResponse<ProductInDetailsDto>>(StatusCodes.Status200OK);



        group.MapPost("/", async (
                [AsParameters] CreateProductRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new ProductCommand(
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Image.OpenReadStream(),
                    request.Image.FileName,
                    request.CategoryName,
                    request.BrandName
                );
                
                var result = await sender.Send(command, ct);

                return result.CommandResult("Product created successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Create product")
            .WithDescription("Returns product ID")
            .Accepts<CreateProductRequest>("multipart/form-data")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created);

        
        group.MapPatch("/", async (
                [AsParameters] UpdateProductRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                
                var command = new UpdateProductCommand(
                    request.Id,
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Image.OpenReadStream(),
                    request.Image.FileName
                );
                
                var result = await sender.Send(command, ct);

                return result.CommandResult(
                    "Product updated successfully",
                    StatusCodes.Status200OK);
            })
            .WithSummary("Update product")
            .WithDescription("Updates an existing product.")
            .Accepts<UpdateProductCommand>("application/json")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        

        group.MapDelete("/{id:guid}", async (
                [AsParameters] IdProductCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult(
                    
                    "Product deleted successfully",
                    StatusCodes.Status204NoContent);
            })
            .WithSummary("Delete product")
            .WithDescription("Deletes an existing product.")
            .Produces(StatusCodes.Status204NoContent);

        
        return endpoints;
    }
}