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

public static class BrandEndpoints
{
    public static IEndpointRouteBuilder MapBrandEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/brands")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));
        
        group.MapGet("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new BrandQuery();
                var result = await sender.Send(query, ct);

                return result.GetListedResults("Brands retrieved successfully");
            })
            .WithSummary("Gets all Brands")
            .WithDescription("Returns a list of Brands (ID, Name)")
            .Produces<ApiResponse<IReadOnlyList<BrandDto>>>(StatusCodes.Status200OK);
        
        
        group.MapPost("/", async (
                NamedBrandCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
               
                
                var result = await sender.Send(command, ct);

                return result.CommandResult("Brand created successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Create Brand")
            .WithDescription("Returns Brand ID")
            .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created);


        return endpoints;
    }
}