using Identity.Application.Auth;
using Identity.Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var ip = GetClientIp();
        return Ok(await _auth.LoginAsync(request,ip));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request)
    {
        var ip = GetClientIp();
        return Ok(await _auth.RefreshAsync(request,ip));
    }
    private string? GetClientIp()
    {
        // Si viene detrás de Gateway/Proxy
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var xff) && !string.IsNullOrWhiteSpace(xff))
            return xff.ToString().Split(',')[0].Trim(); // el primer IP

        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}