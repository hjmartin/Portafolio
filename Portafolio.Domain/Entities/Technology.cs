using Portafolio.Domain.Common;
using Portafolio.Domain.Common.Auditing;

public class Technology : FullAuditedEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string? IconUrl { get; private set; }
    public string? Category { get; private set; }

    public ICollection<ProjectTechnology> Projects { get; private set; } = new List<ProjectTechnology>();

    private Technology() { }

    public static Technology Create(string name, string? iconUrl, string? category)
    {
        var technology = new Technology
        {
            Id = Guid.NewGuid()
        };

        technology.ApplyDetails(name, iconUrl, category);
        return technology;
    }

    public void UpdateDetails(string name, string? iconUrl, string? category)
    {
        ApplyDetails(name, iconUrl, category);
    }

    private void ApplyDetails(string name, string? iconUrl, string? category)
    {
        Name = NormalizeRequired(name, nameof(name), 80);
        IconUrl = NormalizeUrl(iconUrl, nameof(iconUrl), 500);
        Category = NormalizeOptional(category, 60);
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
