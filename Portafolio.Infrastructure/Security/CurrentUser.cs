using Microsoft.AspNetCore.Http;
using Portafolio.Application.Interfaces.Security;
using System.Security.Claims;


namespace Portafolio.Infrastructure.Security;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http) => _http = http;

    public bool IsAuthenticated =>
        _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public string UsuarioId =>
        _http.HttpContext?.User?.FindFirstValue("sub") // recomendado
        ?? _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? "system"; // fallback si corre en background

    public string? Email =>
        _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)
        ?? _http.HttpContext?.User?.FindFirstValue("email");
}

