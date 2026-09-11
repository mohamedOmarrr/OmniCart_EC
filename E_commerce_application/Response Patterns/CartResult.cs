namespace E_commerce_application.Response_Patterns;

public record CartResult<T>(
    Guid CartId,
    IReadOnlyList<T> CartItems,
    int TotalItems,
    decimal SubTotal
    );