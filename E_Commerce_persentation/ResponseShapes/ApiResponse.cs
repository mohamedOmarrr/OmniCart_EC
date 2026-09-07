namespace E_commerce_persentation.ResponseShapes;

public record ApiResponse<T>(
        T? Date,
        string Message
    );