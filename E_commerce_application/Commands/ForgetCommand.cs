using E_commerce_application.DTOs;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record ForgetCommand(string Email) : IRequest<Result<ResponseUserDto>>;