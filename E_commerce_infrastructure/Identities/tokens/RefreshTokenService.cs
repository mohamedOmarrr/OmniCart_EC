using System.Security.Cryptography;
using E_commerce_application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_infrastructure.Identities;

public class RefreshTokenService(AppIdentityDbContext context) : IRefreshTokenService
{
    
    public async Task<string> CreateRefreshTokenAsync(string userId)
    {
        var token = GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
            Revoked = false
        };

        await context.RefreshTokens.AddAsync(refreshToken);

        await context.SaveChangesAsync();

        return token;
    }

    public async Task<(string RefreshToken, string UserId)?>
        RefreshAsync(string refreshToken)
    {
        // Find the old refresh token
        var storedToken = await context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

     
        if (storedToken is null)
            return null;
        
        if (storedToken.ExpiresAt <= DateTimeOffset.UtcNow)
            return null;
        
        if (storedToken.Revoked)
            return null;

        // Get UserId before creating the new token
        var userId = storedToken.UserId;

        // Revoke old token
        storedToken.Revoked = true;

        // Generate new token
        var newRefreshToken = GenerateRefreshToken();

        // Create new record
        var newToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = newRefreshToken,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
            Revoked = false
        };

        await context.RefreshTokens.AddAsync(newToken);
        
        await context.SaveChangesAsync();

        return (newRefreshToken, userId);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }
}