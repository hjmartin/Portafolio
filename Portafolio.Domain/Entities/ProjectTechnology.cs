// ProjectTechnology.cs
public class ProjectTechnology
{
    public Guid ProjectId { get; set; }
    public PortfolioProject Project { get; set; } = default!;

    public Guid TechnologyId { get; set; }
    public Technology Technology { get; set; } = default!;

    // Opcional: metadata de la relación
    // public int Order { get; set; }
    // public string? Level { get; set; } // Junior/Mid/Senior
}