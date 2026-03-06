using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Domain.Common.Auditing.Interface;

public interface IUpdatedEntity
{
    DateTime? LastModifiedAtUtc { get; }
    string? LastModifiedByUserId { get; }
}
