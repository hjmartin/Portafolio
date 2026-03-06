using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Domain.Common.Auditing.Interface;

public interface ICreatedEntity
{
    DateTime CreatedAtUtc { get; }
    string? CreatedByUserId { get; }
}