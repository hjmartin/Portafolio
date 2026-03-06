using Microsoft.EntityFrameworkCore;
using Portafolio.Domain.Common.Auditing.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Infrastructure.Persistence.Extensions;
public static class ModelBuilderAuditExtensions
{
    public static void ApplyAuditColumnOrder(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null) continue;

            var entity = modelBuilder.Entity(clrType);

            // Nombres de columnas auditables (para mandarlas al final)
            var auditNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(ICreatedEntity.CreatedAtUtc),
                nameof(ICreatedEntity.CreatedByUserId),
                nameof(IUpdatedEntity.LastModifiedAtUtc),
                nameof(IUpdatedEntity.LastModifiedByUserId),
                nameof(IDeletableEntity.IsDeleted),
                nameof(IDeletableEntity.DeletedAtUtc),
                nameof(IDeletableEntity.DeletedByUserId),
            };

            // 1) PK primero
            var order = 1;
            var pk = entityType.FindPrimaryKey();
            if (pk is not null)
            {
                foreach (var p in pk.Properties)
                    entity.Property(p.Name).HasColumnOrder(order++);
            }


            // 2) Resto de propiedades respetando el orden de la clase (solo las declaradas en esa entidad)
            var declaredProps = clrType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(p => entityType.FindProperty(p.Name) != null) // solo propiedades mapeadas
                .ToList();

            foreach (var prop in declaredProps)
            {
                if (auditNames.Contains(prop.Name)) continue;
                if (pk?.Properties.Any(x => x.Name == prop.Name) == true) continue;

                entity.Property(prop.Name).HasColumnOrder(order++);
            }


            // 3) Auditoría al final (9000+)
            if (typeof(ICreatedEntity).IsAssignableFrom(clrType))
            {
                entity.Property(nameof(ICreatedEntity.CreatedAtUtc)).HasColumnOrder(9000);
                entity.Property(nameof(ICreatedEntity.CreatedByUserId)).HasColumnOrder(9001);
            }

            if (typeof(IUpdatedEntity).IsAssignableFrom(clrType))
            {
                entity.Property(nameof(IUpdatedEntity.LastModifiedAtUtc)).HasColumnOrder(9010);
                entity.Property(nameof(IUpdatedEntity.LastModifiedByUserId)).HasColumnOrder(9011);
            }

            if (typeof(IDeletableEntity).IsAssignableFrom(clrType))
            {
                entity.Property(nameof(IDeletableEntity.IsDeleted)).HasColumnOrder(9020);
                entity.Property(nameof(IDeletableEntity.DeletedAtUtc)).HasColumnOrder(9021);
                entity.Property(nameof(IDeletableEntity.DeletedByUserId)).HasColumnOrder(9022);
            }
        }
    }
}