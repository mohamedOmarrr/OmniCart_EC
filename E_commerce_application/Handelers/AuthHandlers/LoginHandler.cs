using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.Constants;
using E_commerce_domain.shared;
using E_commerce_infrastructure.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_application.Handelers.AuthHandlers;

public class LoginHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator tokenService,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<LogCommand, Result<RegisterDto>>
{
    public async Task<Result<RegisterDto>> Handle(
        LogCommand request,
        CancellationToken cancellationToken)
    {
        
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result<RegisterDto>.Failure(
                Error.Unauthorized(
                    "User.NotFound",
                    "Failed to Find This User"));
        }

       
        var passwordValid = await userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!passwordValid)
        {
            return Result<RegisterDto>.Failure(
                Error.Unauthorized(
                    "Password.Validation",
                    "the password is Wrong"));
        }

       
        var userTokenData = new UserTokenData(
            user.Id,
            user.Email!,
            user.DisplayName!,
            Roles.Customer);

        var accessToken =
            await tokenService.GenerateToken(userTokenData);

      
        var refreshToken = await refreshTokenService.CreateRefreshTokenAsync(
            user.Id);


        return Result<RegisterDto>.Success(
            new RegisterDto(
                accessToken.ToString(),
                refreshToken));
    }
    
}