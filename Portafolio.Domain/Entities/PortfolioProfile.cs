// PortfolioProfile.cs

using Portafolio.Domain.Common.Auditing;

public class PortfolioProfile: FullAuditedEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Headline { get; set; } = default!;
    public string? Summary { get; set; }

    public string? PhotoUrl { get; set; }
    public string? ResumeUrl { get; set; }

    public string? Location { get; set; }
    public string? PublicEmail { get; set; }

    // 1 -> N (Profile tiene muchos Projects)
    public ICollection<PortfolioProject> Projects { get; set; } = new List<PortfolioProject>();
}