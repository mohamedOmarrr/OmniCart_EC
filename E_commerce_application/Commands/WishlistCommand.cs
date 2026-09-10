using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record WishlistCommand(Guid ProductId): IRequest<Result<Guid>>;