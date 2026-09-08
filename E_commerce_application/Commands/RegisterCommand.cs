using E_commerce_application.DTOs;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record RegisterCommand(
        string DisplayName,
        string Email,
        string Password,
        string PhoneNumber
    ) : IRequest<Result<RegisterDto>>;