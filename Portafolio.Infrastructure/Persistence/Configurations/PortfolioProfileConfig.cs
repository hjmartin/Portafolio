// PortfolioProfileConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Portfolio.Infrastructure.Persistence.Configurations;

public class PortfolioProfileConfig : IEntityTypeConfiguration<PortfolioProfile>
{
    public void Configure(EntityTypeBuilder<PortfolioProfile> builder)
    {
        builder.ToTable("PortfolioProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
               .IsRequired()
               .HasMaxLength(120);

        builder.Property(x => x.Headline)
               .IsRequired()
               .HasMaxLength(160);

        builder.Property(x => x.Summary)
               .HasMaxLength(2000);

        builder.Property(x => x.PhotoUrl)
               .HasMaxLength(500);

        builder.Property(x => x.ResumeUrl)
               .HasMaxLength(500);

        builder.Property(x => x.Location)
               .HasMaxLength(120);

        builder.Property(x => x.PublicEmail)
               .HasMaxLength(256);

  
        // Relación 1 -> N (Profile -> Projects)
        builder.HasMany(x => x.Projects)
               .WithOne(p => p.Profile)
               .HasForeignKey(p => p.ProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        // Si quieres 1 perfil por usuario (solo si UserId NO es null)
        // builder.HasIndex(x => x.UserId).IsUnique();
    }
}