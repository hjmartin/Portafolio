
using Portafolio.Domain.Common.Auditing;

public class Technology : FullAuditedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;   // ".NET", "Angular", "Azure", etc.
    public string? IconUrl { get; set; }
    public string? Category { get; set; }          // "Backend", "Frontend", "DevOps", etc.

    // N:N con Projects (tabla puente)
    public ICollection<ProjectTechnology> Projects { get; set; } = new List<ProjectTechnology>();
}