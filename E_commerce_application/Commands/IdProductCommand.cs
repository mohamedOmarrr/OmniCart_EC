using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record IdProductCommand(Guid Id): IRequest<Result>;