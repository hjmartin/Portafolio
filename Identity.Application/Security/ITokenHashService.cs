using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Security
{
    public interface ITokenHashService
    {
        string Hash(string token);
    }
}
