using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> b)
    {
        b.ToTable("Roles");

        b.HasKey(x => x.Id);

        b.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        b.HasIndex(x => x.Name)
            .IsUnique();
    }
}