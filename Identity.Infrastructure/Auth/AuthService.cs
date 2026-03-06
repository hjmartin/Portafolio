using Identity.Application.Auth;
using Identity.Application.Auth.Dtos;
using Identity.Application.Security;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Auth;

public sealed class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ITokenHashService _tokenHash;
    private readonly IConfiguration _config;

    public AuthService(
        ApplicationDbContext db,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ITokenHashService tokenHash,
        IConfiguration config)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _tokenHash = tokenHash;
        _config = config;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, string? ip)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _db.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var roles = user.UserRoles.Select(x => x.Role.Name).ToList();
        var (access, accessExp) = _tokenService.CreateAccessToken(user, roles);

        var refreshPlain = _tokenService.CreateRefreshToken();
        var refreshHash = _tokenHash.Hash(refreshPlain);

        var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"] ?? "15");
        var refreshExp = DateTime.UtcNow.AddDays(refreshDays);

        // (Opcional recomendado) limitar refresh activos por usuario
        // Ej: mantener máximo 5 refresh activos
        var maxActive = 5;
        var activeTokens = await _db.RefreshTokens
            .Where(t => t.UserId == user.Id && t.RevokedAtUtc == null && t.ExpiresAtUtc > DateTime.UtcNow)
            .OrderBy(t => t.CreatedAtUtc)
            .ToListAsync();

        if (activeTokens.Count >= maxActive)
        {
            // revoca los más viejos
            foreach (var old in activeTokens.Take(activeTokens.Count - (maxActive - 1)))
            {
                old.RevokedAtUtc = DateTime.UtcNow;
                old.RevokedByIp = ip;
            }
        }

        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshHash,
            ExpiresAtUtc = refreshExp,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByIp = ip
        });

        await _db.SaveChangesAsync();

        return new AuthResponse(access, accessExp, refreshPlain, refreshExp);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request, string? ip)
    {
        var incomingHash = _tokenHash.Hash(request.RefreshToken);

        await using var tx = await _db.Database.BeginTransactionAsync();

        var token = await _db.RefreshTokens
            .Include(t => t.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(t => t.TokenHash == incomingHash);

        // Si no existe, malo
        if (token is null)
            throw new UnauthorizedAccessException("Refresh token inválido o expirado.");

        // Si el usuario está inactivo, malo
        if (!token.User.IsActive)
            throw new UnauthorizedAccessException("Refresh token inválido o expirado.");

        // Reuse detection: token existe pero NO está activo (revocado/expirado)
        if (!token.IsActive)
        {
            // señal fuerte de robo/uso repetido: revoca todos los refresh activos del usuario
            var now = DateTime.UtcNow;

            var actives = await _db.RefreshTokens
                .Where(t => t.UserId == token.UserId && t.RevokedAtUtc == null && t.ExpiresAtUtc > now)
                .ToListAsync();

            foreach (var t in actives)
            {
                t.RevokedAtUtc = now;
                t.RevokedByIp = ip;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            throw new UnauthorizedAccessException("Refresh token inválido o expirado.");
        }

        // ROTACIÓN (one-time): revoca el actual dentro de la tx
        token.RevokedAtUtc = DateTime.UtcNow;
        token.RevokedByIp = ip;

        var user = token.User;
        var roles = user.UserRoles.Select(x => x.Role.Name).ToList();

        var (access, accessExp) = _tokenService.CreateAccessToken(user, roles);

        var newRefreshPlain = _tokenService.CreateRefreshToken();
        var newRefreshHash = _tokenHash.Hash(newRefreshPlain);

        var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"] ?? "15");
        var newRefreshExp = DateTime.UtcNow.AddDays(refreshDays);

        var newToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = newRefreshHash,
            ExpiresAtUtc = newRefreshExp,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByIp = ip,
            ReplacesTokenId = token.Id
        };

        _db.RefreshTokens.Add(newToken);

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return new AuthResponse(access, accessExp, newRefreshPlain, newRefreshExp);
    }
}