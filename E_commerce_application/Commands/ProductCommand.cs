using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Commands;

public record ProductCommand(
        string Name,
        string Description,
        decimal Price,
        Stream StreamImage,
        string FileName,
        string CategoryName,
        string BrandName
    ): IRequest<Result<Guid>>;