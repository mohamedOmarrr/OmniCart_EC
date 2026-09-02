namespace E_commerce_application.Interfaces;

public interface IRefreshTokenService
{
    Task<string> CreateRefreshTokenAsync(string userId);

    Task<(string RefreshToken, string UserId)?>
        RefreshAsync(string refreshToken);
}