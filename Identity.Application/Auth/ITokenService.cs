using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Auth
{
    public interface ITokenService
    {
        (string token, DateTime expiresAtUtc) CreateAccessToken(User user, IReadOnlyList<string> roles);
        string CreateRefreshToken(); // token plano (se guarda hash en DB)
    }
}
