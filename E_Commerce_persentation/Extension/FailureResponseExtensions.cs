using E_commerce_domain.shared;

namespace E_Commerce_persentation.Extensions;

public static class FailureResponseExtensions
{
    public static IResult ToProblemDetails(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation =>
                Results.BadRequest(error),

            ErrorType.NotFound =>
                Results.NotFound(error),

            ErrorType.Conflict =>
                Results.Conflict(error),

            ErrorType.Unauthorized =>
                Results.Json(
                    error,
                    statusCode: StatusCodes.Status401Unauthorized),

            ErrorType.Forbidden =>
                Results.Json(
                    error,
                    statusCode: StatusCodes.Status403Forbidden),

            ErrorType.Failure =>
                Results.Json(
                    error,
                    statusCode: StatusCodes.Status500InternalServerError),

            _ =>
                Results.Json(
                    error,
                    statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}