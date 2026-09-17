using E_commerce_application.Commands;
using E_commerce_application.DTOs;
using E_commerce_application.Interfaces;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Handelers.UserHandlers;

public class VerifyCodeHandler(
    IEmailVerificationCodeStore verificationCodeStore)
    : IRequestHandler<VerifyCommand, Result<ResponseUserDto>>
{
    public async Task<Result<ResponseUserDto>> Handle(
        VerifyCommand request,
        CancellationToken cancellationToken)
    {
        var isValid =
            await verificationCodeStore.ValidateAsync(
                request.UserId,
                request.Code);

        if (!isValid)
        {
            return Result<ResponseUserDto>.Failure(
                Error.Validation("verificationCode.Invaild",  "The verificationCode is invalid or expired")
                );
        }

        await verificationCodeStore.RemoveAsync(
            request.UserId);

        return Result<ResponseUserDto>.Success(new ResponseUserDto("Success"));
    }
}