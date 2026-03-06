using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Common.Exceptions;

public abstract class AppException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }

    protected AppException(string message, int statusCode, string code)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;

    }
}
