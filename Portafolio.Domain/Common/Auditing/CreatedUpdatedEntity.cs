using Portafolio.Domain.Common.Auditing.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Domain.Common.Auditing
{
    public abstract class CreateUpdateEntity
        : CreatedEntity, IUpdatedEntity
    {
        public DateTime? LastModifiedAtUtc { get; protected set; }
        public string? LastModifiedByUserId { get; protected set; }

        public void SetModifiedAudit(DateTime nowUtc, string userId)
        {
            LastModifiedAtUtc = nowUtc;
            LastModifiedByUserId = userId;
        }
    }
}
