using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_commerce_application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace E_commerce_infrastructure.Identities;

public class JwtTokenGenerator(IOptions<JwtSettings> settings) : IJwtTokenGenerator
{
    
    private readonly JwtSettings _settings = settings.Value;
    
    public async Task<AccessTokenResult> GenerateToken(UserTokenData userToken)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userToken.UserId.ToString()),
            new(ClaimTypes.NameIdentifier, userToken.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, userToken.Email),
            new(ClaimTypes.Email, userToken.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrWhiteSpace(userToken.DisplayName))
            claims.Add(new Claim("display_name", userToken.DisplayName));

        claims.Add( new Claim(ClaimTypes.Role, userToken.Role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var written = new JwtSecurityTokenHandler().WriteToken(token);
        return new AccessTokenResult(written, expiresAt);
    }
}