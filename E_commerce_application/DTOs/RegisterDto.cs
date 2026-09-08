namespace E_commerce_application.DTOs;

public record RegisterDto(
        string AccessToken,
        string RefreshToken
    );