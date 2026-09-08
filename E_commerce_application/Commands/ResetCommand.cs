using System.Windows.Input;
using E_commerce_application.DTOs;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record ResetCommand(string UserId, string? Password) : IRequest<Result<RegisterDto>>;