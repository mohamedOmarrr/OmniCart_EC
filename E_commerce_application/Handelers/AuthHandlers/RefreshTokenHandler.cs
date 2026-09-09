using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.Constants;
using E_commerce_domain.shared;
using E_commerce_infrastructure.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_application.Handelers.AuthHandlers;

public class RefreshTokenHandler(
    IRefreshTokenService refreshTokenService,
    ITokenService tokenService,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<RefreshCommand, Result<RegisterDto>>
{
    public async Task<Result<RegisterDto>> Handle(
        RefreshCommand request,
        CancellationToken cancellationToken)
    {
        
        var response = await refreshTokenService.RefreshAsync(
            request.RefreshToken);

        if (response is null)
        {
            return Result<RegisterDto>.Failure(
                Error.Failure(
                    "RefreshToken.NotValid",
                    "Failed to Create Tokens"));
        }
        
        var user = await userManager.FindByIdAsync(response.Value.UserId);
        
        if (user is null)
        {
            return Result<RegisterDto>.Failure(
                Error.Failure(
                    "User.NotFound",
                    "Failed to Find This User"));
        }

        var userTokenData = new UserTokenData(
            user.Id,
            user.Email!,
            user.DisplayName!,
            Roles.Customer);

        var accessToken =
            await tokenService.CreateTokenAsync(userTokenData);

      
        var refreshToken = response.Value.RefreshToken;

        return Result<RegisterDto>.Success(
            new RegisterDto(
                accessToken,
                refreshToken));
    }
}