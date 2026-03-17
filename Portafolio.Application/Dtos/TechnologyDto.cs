namespace Portafolio.Application.Dtos;

public class TechnologyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Category { get; set; }
}
