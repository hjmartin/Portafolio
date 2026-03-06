using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Dtos
{
    public class PortfolioProfileDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Headline { get; set; } = default!;
        public string? Summary { get; set; }
        public string? PhotoUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public string? Location { get; set; }
        public string? PublicEmail { get; set; }
        public List<PortfolioProjectDto> Projects { get; set; } = new();
    }
}
