using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.shared;
using E_commerce_infrastructure.Identities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_application.Handelers.UserHandlers;

public class ForgotPasswordHandler(
    UserManager<ApplicationUser> userManager,
    IEmailVerificationCodeStore verificationCodeStore,
    IEmailService emailService)
    : IRequestHandler<ForgetCommand, Result<ResponseUserDto>>
{
    public async Task<Result<ResponseUserDto>> Handle(
        ForgetCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(
            request.Email);

        if (user is null)
        {
            return Result<ResponseUserDto>.Failure(Error.NotFound("User.NotFound",  "UnCorrect Email"));
        }

        var code = verificationCodeStore.CreateCode();

        await verificationCodeStore.StoreAsync(
            user.Id.ToString(),
            code,
            TimeSpan.FromMinutes(5));

        await emailService.SendEmailAsync(
            user.Email!,
            "Password Reset Verification Code",
            $"Your verification code is: <strong>{code}</strong>");

        return Result<ResponseUserDto>.Success(new ResponseUserDto("Success"));
    }
}