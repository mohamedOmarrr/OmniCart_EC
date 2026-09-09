using Asp.Versioning;
using Asp.Versioning.Builder;
using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_domain.shared;
using E_Commerce_persentation.Extensions;
using E_commerce_persentation.ResponseShapes;
using MediatR;

namespace E_Commerce_persentation.EndPoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/user")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0));
     
        group.MapPost("/register", async (
                [AsParameters] RegisterCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("Your account has been Created successfully", StatusCodes.Status201Created);
            })
            .WithSummary("Register a User")
            .WithDescription("Returns AccessToken and RefreshToken After Registration Succeeded")
            .Produces<ApiResponse<RegisterDto>>(StatusCodes.Status201Created);
        
        
        group.MapPost("/forget", async (
                [AsParameters] ForgetCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("Your Code Has Been Send", StatusCodes.Status200OK);
            })
            .WithSummary("Forget Password")
            .WithDescription("send verification code email to User")
            .Produces<ApiResponse<ResponseUserDto>>(StatusCodes.Status200OK);
        
        
        group.MapPost("/verify", async (
                [AsParameters] VerifyCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("Your Code has Been Verified successfully", StatusCodes.Status200OK);
            })
            .WithSummary("verify code email")
            .WithDescription("Returns Message that Your Code has Been Verified")
            .Produces<ApiResponse<RegisterDto>>(StatusCodes.Status200OK);
        
        
        group.MapPost("/reset", async (
                [AsParameters] ResetCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.CommandResult("You Are Logged/Reset in successfully", StatusCodes.Status200OK);
            })
            .WithSummary("Reset Password")
            .WithDescription("Returns Access and Refresh Token with message")
            .Produces<ApiResponse<RegisterDto>>(StatusCodes.Status200OK);
        
        
        return endpoints;
    }

}