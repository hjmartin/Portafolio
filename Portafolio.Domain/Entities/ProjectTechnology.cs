public class ProjectTechnology
{
    public Guid ProjectId { get; private set; }
    public PortfolioProject Project { get; private set; } = default!;

    public Guid TechnologyId { get; private set; }
    public Technology Technology { get; private set; } = default!;

    private ProjectTechnology() { }

    private ProjectTechnology(Guid projectId, Guid technologyId)
    {
        ProjectId = projectId;
        TechnologyId = technologyId;
    }

    public static ProjectTechnology Create(Guid projectId, Guid technologyId)
        => new(projectId, technologyId);
}
