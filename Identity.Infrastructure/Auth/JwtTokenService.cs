using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Application.Auth;
using Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Auth;

public sealed class JwtTokenService : ITokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config) => _config = config;

    public (string token, DateTime expiresAtUtc) CreateAccessToken(User user, IReadOnlyList<string> roles)
    {
        var issuer = _config["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Missing configuration: Jwt:Issuer");
        var audience = _config["Jwt:Audience"]
            ?? throw new InvalidOperationException("Missing configuration: Jwt:Audience");
        var key = _config["Jwt:Key"]
            ?? throw new InvalidOperationException("Missing configuration: Jwt:Key");

        if (!int.TryParse(_config["Jwt:AccessTokenMinutes"], out var minutes) || minutes <= 0)
            minutes = 15;

        if (Encoding.UTF8.GetByteCount(key) < 32)
            throw new InvalidOperationException("Jwt:Key must be at least 32 bytes.");

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(minutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var r in roles)
        {
            claims.Add(new Claim("role", r));
            claims.Add(new Claim(ClaimTypes.Role, r));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now.AddMinutes(-1),
            expires: expires,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
    }

    public string CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}

