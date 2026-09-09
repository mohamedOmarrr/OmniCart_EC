using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_Commerce_persentation.Extensions;
using E_commerce_persentation.ResponseShapes;
using MediatR;

namespace E_Commerce_persentation.EndPoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/auth")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));
        
        group.MapPost("/log", async (
                [AsParameters] LogCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("You are Login successfully", StatusCodes.Status200OK);
            })
            .WithSummary("Login with Account")
            .WithDescription("Returns AccessToken and RefreshToken After Login Succeeded")
            .Produces<ApiResponse<RegisterDto>>(StatusCodes.Status200OK);
        
        group.MapPost("/refresh", async (
                [AsParameters] RefreshCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("Create Tokens Done Successfully", StatusCodes.Status200OK);
            })
            .WithSummary("Refresh Tokens")
            .WithDescription("Returns AccessToken and RefreshToken when AccessToken is Expired")
            .Produces<ApiResponse<RegisterDto>>(StatusCodes.Status200OK);

        return endpoints;
    }
}