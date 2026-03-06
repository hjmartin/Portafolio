using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message, string code = "not_found")
        : base(message, 404, code)
    {
    }
}
