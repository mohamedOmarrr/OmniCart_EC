using E_commerce_application.DTOs;
using E_commerce_application.Response_Patterns;
using E_commerce_domain.shared;
using MediatR;

namespace E_commerce_application.Queries;

public record ProductPagedQuery(
        int PageNumber,
        int PageSize,
        string? Search,
        string? TypeOfSearch
    ): IRequest<Result<PagedResult<ProductDTO>>>;