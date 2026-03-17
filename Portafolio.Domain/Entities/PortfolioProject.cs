using System.Text.RegularExpressions;
using Portafolio.Domain.Common;
using Portafolio.Domain.Common.Auditing;

public class PortfolioProject : FullAuditedEntity
{
    public Guid Id { get; private set; }
    public Guid ProfileId { get; private set; }
    public PortfolioProfile Profile { get; private set; } = default!;

    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? LiveUrl { get; private set; }
    public string? RepoUrl { get; private set; }
    public string? CoverImageUrl { get; private set; }
    public bool IsFeatured { get; private set; }
    public int SortOrder { get; private set; }

    public ICollection<ProjectTechnology> Technologies { get; private set; } = new List<ProjectTechnology>();

    private PortfolioProject() { }

    public static PortfolioProject Create(
        Guid profileId,
        string title,
        string? description,
        string? liveUrl,
        string? repoUrl,
        string? coverImageUrl,
        bool isFeatured,
        int sortOrder)
    {
        if (profileId == Guid.Empty)
            throw new DomainRuleException("ProfileId is required.");

        var project = new PortfolioProject
        {
            Id = Guid.NewGuid(),
            ProfileId = profileId
        };

        project.ApplyDetails(title, description, liveUrl, repoUrl, coverImageUrl, isFeatured, sortOrder);
        return project;
    }

    public void UpdateDetails(
        string title,
        string? description,
        string? liveUrl,
        string? repoUrl,
        string? coverImageUrl,
        bool isFeatured,
        int sortOrder)
    {
        ApplyDetails(title, description, liveUrl, repoUrl, coverImageUrl, isFeatured, sortOrder);
    }

    public void AddTechnology(Guid technologyId)
    {
        if (technologyId == Guid.Empty)
            throw new DomainRuleException("TechnologyId is required.");

        if (Technologies.Any(x => x.TechnologyId == technologyId))
            throw new DomainRuleException("Technology is already linked to this project.");

        Technologies.Add(ProjectTechnology.Create(Id, technologyId));
    }

    public void RemoveTechnology(Guid technologyId)
    {
        var relation = Technologies.FirstOrDefault(x => x.TechnologyId == technologyId);
        if (relation is null)
            throw new DomainRuleException("Technology is not linked to this project.");

        Technologies.Remove(relation);
    }

    private void ApplyDetails(
        string title,
        string? description,
        string? liveUrl,
        string? repoUrl,
        string? coverImageUrl,
        bool isFeatured,
        int sortOrder)
    {
        Title = NormalizeRequired(title, nameof(title), 160);
        Description = NormalizeOptional(description, 4000);
        LiveUrl = NormalizeUrl(liveUrl, nameof(liveUrl), 500);
        RepoUrl = NormalizeUrl(repoUrl, nameof(repoUrl), 500);
        CoverImageUrl = NormalizeUrl(coverImageUrl, nameof(coverImageUrl), 500);

        if (sortOrder < 0)
            throw new DomainRuleException("SortOrder cannot be negative.");

        IsFeatured = isFeatured;
        SortOrder = sortOrder;
    }

    private static string NormalizeRequired(string? value, string field, int maxLength)
    {
        var normalized = (value ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new DomainRuleException($"{field} is required.");

        if (normalized.Length > maxLength)
            throw new DomainRuleException($"{field} max length is {maxLength}.");

        return normalized;
    }

    private static string? NormalizeOptional(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
            throw new DomainRuleException($"Field max length is {maxLength}.");

        return normalized;
    }

    private static string? NormalizeUrl(string? value, string field, int maxLength)
    {
        var normalized = NormalizeOptional(value, maxLength);
        if (normalized is null)
            return null;

        if (!Uri.TryCreate(normalized, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainRuleException($"{field} must be a valid http/https URL.");
        }

        return normalized;
    }
}
