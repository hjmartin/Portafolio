namespace Portafolio.Application.Dtos;

public class PortfolioProjectDto
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? LiveUrl { get; set; }
    public string? RepoUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
    public List<TechnologyDto> Technologies { get; set; } = new();
}
