namespace E_commerce_application.Interfaces;

public interface IRefreshTokenService
{
    Task<string> CreateRefreshTokenAsync(Guid userId);

    Task<(string RefreshToken, string UserId)?>
        RefreshAsync(string refreshToken);
}