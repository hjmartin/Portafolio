using Portafolio.Domain.Common.Auditing.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Domain.Common.Auditing;

public abstract class CreatedEntity : ICreatedEntity
{
    public DateTime CreatedAtUtc { get; protected set; }
    public string? CreatedByUserId { get; protected set; }

    public void SetCreatedAudit(DateTime nowUtc, string userId)
    {
        // idempotente por si EF reintenta
        if (CreatedAtUtc == default)
            CreatedAtUtc = nowUtc;

        if (string.IsNullOrWhiteSpace(CreatedByUserId))
            CreatedByUserId = userId;
    }
}
