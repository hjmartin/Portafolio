using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Dtos;

public class CreatePortfolioProfileDto
{
    public string FullName { get; set; } = default!;
    public string Headline { get; set; } = default!;
    public string? Summary { get; set; }
    public string? Location { get; set; }
    public string? PublicEmail { get; set; }
}
