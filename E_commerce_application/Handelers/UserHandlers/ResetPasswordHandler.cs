using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.Constants;
using E_commerce_domain.shared;
using E_commerce_infrastructure.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_application.Handelers.UserHandlers;

public class ResetPasswordHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator tokenService,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<ResetCommand, Result<RegisterDto>>
{
    public async Task<Result<RegisterDto>> Handle(
        ResetCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(
            request.UserId);

        if (user is null)
        {
                return Result<RegisterDto>.Failure(
                    Error.NotFound(
                        "User.NotFound",
                        "Failed to find the user"));
        }

        if (request.Password is not null)
        {
            await userManager.RemovePasswordAsync(user);
            
            
            var addResult =
                await userManager.AddPasswordAsync(
                    user,
                    request.Password);

           
                if (!addResult.Succeeded)
                {
                    var errors = addResult.Errors
                        .Select(e => e.Description)
                        .ToList();

                    return Result<RegisterDto>.Failure(
                        Error.Validation(
                            "Password.ResetFailed",
                            string.Join(", ", errors)));
                }
           
        }
        
        var role = await userManager.GetRolesAsync(user);

        var userTokenData = new UserTokenData(
            user.Id,
            user.Email!,
            user.DisplayName!,
            role.FirstOrDefault());

        var accessToken =
            await tokenService.GenerateToken(userTokenData);

        var refreshToken =
            await refreshTokenService.CreateRefreshTokenAsync(
                user.Id);

        return Result<RegisterDto>.Success(new RegisterDto (accessToken.ToString(), refreshToken));
    }
}