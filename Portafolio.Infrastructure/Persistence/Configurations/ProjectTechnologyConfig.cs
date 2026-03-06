// ProjectTechnologyConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Portfolio.Infrastructure.Persistence.Configurations;

public class ProjectTechnologyConfig : IEntityTypeConfiguration<ProjectTechnology>
{
    public void Configure(EntityTypeBuilder<ProjectTechnology> builder)
    {
        builder.ToTable("ProjectTechnologies");

        // PK compuesta (evita duplicados ProjectId + TechnologyId)
        builder.HasKey(x => new { x.ProjectId, x.TechnologyId });

        builder.HasOne(x => x.Project)
               .WithMany(p => p.Technologies)
               .HasForeignKey(x => x.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Technology)
               .WithMany(t => t.Projects)
               .HasForeignKey(x => x.TechnologyId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TechnologyId);
    }
}