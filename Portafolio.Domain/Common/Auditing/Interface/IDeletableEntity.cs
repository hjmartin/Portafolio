using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Domain.Common.Auditing.Interface;
public interface IDeletableEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedAtUtc { get; }
    string? DeletedByUserId { get; }
}
