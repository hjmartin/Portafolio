using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Interfaces.Security
{
    public interface ICurrentUser
    {
        string UsuarioId { get; }       // o Guid UsuarioId
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}
