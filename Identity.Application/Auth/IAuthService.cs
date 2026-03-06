using Identity.Application.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request, string? ip);
        Task<AuthResponse> RefreshAsync(RefreshRequest request, string? ip);
    }
}
