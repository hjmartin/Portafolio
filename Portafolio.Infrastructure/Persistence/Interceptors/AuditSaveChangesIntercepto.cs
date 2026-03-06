using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Portafolio.Application.Interfaces.Security;
using Portafolio.Domain.Common.Auditing;
using Portafolio.Domain.Common.Auditing.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;

    public AuditSaveChangesInterceptor(ICurrentUser currentUser)
        => _currentUser = currentUser;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context is null) return;

        var now = DateTime.UtcNow;
        var userId = string.IsNullOrWhiteSpace(_currentUser.UsuarioId) ? "system" : _currentUser.UsuarioId;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // CREATE
            if (entry.State == EntityState.Added && entry.Entity is CreatedEntity created)
            {
                created.SetCreatedAudit(now, userId);
                continue;
            }

            // UPDATE
            if (entry.State == EntityState.Modified && entry.Entity is CreateUpdateEntity updated)
            {
                updated.SetModifiedAudit(now, userId);
            }

            // DELETE -> Soft delete
            if (entry.State == EntityState.Deleted && entry.Entity is FullAuditedEntity deletable)
            {
                entry.State = EntityState.Modified;
                deletable.SetDeletedAudit(now, userId);
            }
        }
    }
}
