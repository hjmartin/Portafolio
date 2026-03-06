// PortfolioProjectConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Portfolio.Infrastructure.Persistence.Configurations;

public class PortfolioProjectConfig : IEntityTypeConfiguration<PortfolioProject>
{
    public void Configure(EntityTypeBuilder<PortfolioProject> builder)
    {
        builder.ToTable("PortfolioProjects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(160);

        builder.Property(x => x.Description)
               .HasMaxLength(4000);

        builder.Property(x => x.LiveUrl)
               .HasMaxLength(500);

        builder.Property(x => x.RepoUrl)
               .HasMaxLength(500);

        builder.Property(x => x.CoverImageUrl)
               .HasMaxLength(500);

        builder.Property(x => x.SortOrder)
               .HasDefaultValue(0);

        // Índices útiles
        builder.HasIndex(x => x.ProfileId);
        builder.HasIndex(x => new { x.ProfileId, x.IsFeatured });
    }
}