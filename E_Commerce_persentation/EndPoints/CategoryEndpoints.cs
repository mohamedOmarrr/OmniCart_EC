using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Queries;
using E_Commerce_persentation.Extensions;
using E_commerce_persentation.ResponseShapes;
using MediatR;

namespace E_Commerce_persentation.EndPoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/category")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapGet("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new EmptyCategoryQuery();
                var result = await sender.Send(query, ct);

                return result.GetListedResults("Categories retrieved successfully");
            })
            .WithSummary("Gets all Categories")
            .WithDescription("Returns a list of Categories (ID, Name)")
            .Produces<ApiResponse<IReadOnlyList<CategoryDto>>>(StatusCodes.Status200OK);
        
        
        group.MapPost("/", async (
                [AsParameters] NamedCategoryCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
               
                
                var result = await sender.Send(command, ct);

                return result.CommandResult("Category created successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Create Category")
            .WithDescription("Returns Category ID")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created);


        return endpoints;
    }
}