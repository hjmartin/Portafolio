using System.Net.Mail;
using Portafolio.Domain.Common;
using Portafolio.Domain.Common.Auditing;

public class PortfolioProfile : FullAuditedEntity
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = default!;
    public string Headline { get; private set; } = default!;
    public string? Summary { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string? ResumeUrl { get; private set; }
    public string? Location { get; private set; }
    public string? PublicEmail { get; private set; }

    public ICollection<PortfolioProject> Projects { get; private set; } = new List<PortfolioProject>();

    private PortfolioProfile() { }

    public static PortfolioProfile Create(
        string fullName,
        string headline,
        string? summary,
        string? location,
        string? publicEmail,
        string? photoUrl = null,
        string? resumeUrl = null)
    {
        var profile = new PortfolioProfile
        {
            Id = Guid.NewGuid()
        };

        profile.ApplyDetails(fullName, headline, summary, location, publicEmail, photoUrl, resumeUrl);
        return profile;
    }

    public void UpdateDetails(
        string fullName,
        string headline,
        string? summary,
        string? location,
        string? publicEmail,
        string? photoUrl,
        string? resumeUrl)
    {
        ApplyDetails(fullName, headline, summary, location, publicEmail, photoUrl, resumeUrl);
    }

    private void ApplyDetails(
        string fullName,
        string headline,
        string? summary,
        string? location,
        string? publicEmail,
        string? photoUrl,
        string? resumeUrl)
    {
        FullName = NormalizeRequired(fullName, nameof(fullName), 120);
        Headline = NormalizeRequired(headline, nameof(headline), 160);
        Summary = NormalizeOptional(summary, 2000);
        Location = NormalizeOptional(location, 120);
        PhotoUrl = NormalizeOptional(photoUrl, 500);
        ResumeUrl = NormalizeOptional(resumeUrl, 500);

        var normalizedEmail = NormalizeOptional(publicEmail, 256);
        if (!string.IsNullOrWhiteSpace(normalizedEmail))
        {
            try
            {
                _ = new MailAddress(normalizedEmail);
            }
            catch
            {
                throw new DomainRuleException("PublicEmail format is invalid.");
            }
        }

        PublicEmail = normalizedEmail;
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
}
