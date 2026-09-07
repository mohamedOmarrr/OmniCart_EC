namespace E_commerce_application.Response_Patterns;

public record PagedResult<T>(
        IReadOnlyList<T> Items,
        int PageNumber,
        int PageSize,
        int TotalPages,
        int TotalCount
    );