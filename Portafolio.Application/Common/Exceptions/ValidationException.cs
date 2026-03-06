using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Common.Exceptions;

public sealed class ValidationException : AppException
{
    public ValidationException(string message, string code = "validation_error")
        : base(message, 400, code)
    {
    }
}