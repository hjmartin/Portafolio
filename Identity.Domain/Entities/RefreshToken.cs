using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public string TokenHash { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; } // opcional

        public DateTime ExpiresAtUtc { get; set; }

        public DateTime? RevokedAtUtc { get; set; }
        public string? RevokedByIp { get; set; }

        public Guid? ReplacesTokenId { get; set; } // link al token anterior

        public bool IsActive =>
            !RevokedAtUtc.HasValue &&
            DateTime.UtcNow < ExpiresAtUtc;
    }
}
