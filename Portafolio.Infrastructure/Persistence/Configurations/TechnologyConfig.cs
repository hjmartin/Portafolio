// TechnologyConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Portfolio.Infrastructure.Persistence.Configurations;

public class TechnologyConfig : IEntityTypeConfiguration<Technology>
{
    public void Configure(EntityTypeBuilder<Technology> builder)
    {
        builder.ToTable("Technologies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(80);

        builder.Property(x => x.IconUrl)
               .HasMaxLength(500);

        builder.Property(x => x.Category)
               .HasMaxLength(60);

        // Evitar duplicados: ".NET" repetido, etc.
        builder.HasIndex(x => x.Name).IsUnique();


    }
}