using E_commerce_application.Response_Patterns;
using E_commerce_domain.shared;
using E_commerce_persentation.ResponseShapes;

namespace E_Commerce_persentation.Extensions;

public static class ResultResponseExtensions
{
    public static IResult GetPagedResults<T>(
        this Result<PagedResult<T>> result,
        string message)
    {
        if (result.IsFailure)
            return result.Error.ToProblemDetails();

        var value = result.Value;

        return Results.Ok(
            new ApiResponse<PagedResult<T>>(
                value,
                message));
    }

    public static IResult GetSpecificItem<T>(
        this Result<T> result,
        string message)
    {
        if (result.IsFailure)
            return result.Error.ToProblemDetails();

        return Results.Ok(
            new ApiResponse<T>(
                result.Value,
                message));
    }

    public static IResult CommandResult(
        this Result result,
        string message,
        int successStatusCode)
    {
        if (result.IsFailure)
            return result.Error.ToProblemDetails();

        if (successStatusCode == StatusCodes.Status204NoContent)
            return Results.NoContent();

        return Results.Json(
            new ApiResponse<object?>(
                null,
                message),
            statusCode: successStatusCode);
    }

    public static IResult CommandResult<TValue>(
        this Result<TValue> result,
        string message,
        int successStatusCode)
    {
        if (result.IsFailure)
            return result.Error.ToProblemDetails();

        return Results.Json(
            new ApiResponse<TValue>(
                result.Value,
                message),
            statusCode: successStatusCode);
    }
}