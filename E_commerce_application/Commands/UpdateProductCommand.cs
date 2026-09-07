using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record UpdateProductCommand(
        Guid Id,
        string? Name,
        string? Description,
        decimal? Price,
        Stream? ImageStream,
        string? FileName
    ): IRequest<Result<Guid>>;