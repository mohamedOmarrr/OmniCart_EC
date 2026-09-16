using E_commerce_domain.shared;

namespace E_Commerce_persentation.Extensions;

public static class FailureResponseExtensions
{
    public static IResult ToProblemDetails(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(error),
            ErrorType.NotFound => Results.NotFound(error),
            ErrorType.Conflict => Results.Conflict(error),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            ErrorType.Failure => Results.StatusCode(500),
            _ => Results.StatusCode(500)
        };
    }
}