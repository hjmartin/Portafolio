namespace Portafolio.Application.Dtos;

public class CreatePortfolioProjectDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? LiveUrl { get; set; }
    public string? RepoUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
}
