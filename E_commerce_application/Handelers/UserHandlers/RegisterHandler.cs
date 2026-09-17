using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.Constants;
using E_commerce_domain.shared;
using E_commerce_infrastructure.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_application.Handelers.UserHandlers;

public class RegisterHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator tokenService,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<RegisterCommand, Result<RegisterDto>>
{
    public async Task<Result<RegisterDto>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var result = await userManager.CreateAsync(
            user,
            request.Password);

        
        if (!result.Succeeded)
        {
            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => e.Description)
                    .ToList();

                return Result<RegisterDto>.Failure(
                    Error.Validation(
                        "User.CreateFailed",
                        string.Join(", ", errors)));
            }
        }

        var role = await userManager.AddToRoleAsync(user, Roles.Customer);
        
        if (!role.Succeeded)
        {
            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => e.Description)
                    .ToList();

                return Result<RegisterDto>.Failure(
                    Error.Validation(
                        "User.RoleAssignmentFailed",
                        string.Join(", ", errors)));
            }
        }
        
        var userTokenData = new UserTokenData(
            user.Id,
            user.Email!,
            user.DisplayName!,
            Roles.Customer);

        var accessToken = await tokenService.GenerateToken(userTokenData);

        var refreshToken =
            await refreshTokenService.CreateRefreshTokenAsync(
                user.Id);

        return Result<RegisterDto>.Success(
            new RegisterDto(
                accessToken.ToString(),
                refreshToken));
    }
}