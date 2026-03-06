using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string passwordHash) =>
            BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
