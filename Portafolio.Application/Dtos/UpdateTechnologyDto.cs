namespace Portafolio.Application.Dtos;

public class UpdateTechnologyDto
{
    public string Name { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Category { get; set; }
}
