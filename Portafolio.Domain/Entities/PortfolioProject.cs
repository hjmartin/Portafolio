// PortfolioProject.cs

using Portafolio.Domain.Common.Auditing;

public class PortfolioProject : FullAuditedEntity
{
    public Guid Id { get; set; }

    // FK hacia PortfolioProfile (para que no queden proyectos huérfanos)
    public Guid ProfileId { get; set; }
    public PortfolioProfile Profile { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public string? LiveUrl { get; set; }
    public string? RepoUrl { get; set; }
    public string? CoverImageUrl { get; set; }

    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }

    // N:N con Technology (tabla puente)
    public ICollection<ProjectTechnology> Technologies { get; set; } = new List<ProjectTechnology>();
}