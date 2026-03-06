using Portafolio.Domain.Common.Auditing.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Domain.Common.Auditing;

public abstract class FullAuditedEntity
    : CreateUpdateEntity, IDeletableEntity
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAtUtc { get; protected set; }
    public string? DeletedByUserId { get; protected set; }

    public void SetDeletedAudit(DateTime nowUtc, string userId)
    {
        IsDeleted = true;
        DeletedAtUtc = nowUtc;
        DeletedByUserId = userId;
    }
}
