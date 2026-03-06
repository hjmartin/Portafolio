using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Common.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message, string code = "conflict")
        : base(message, 409, code)
    {
    }
}
