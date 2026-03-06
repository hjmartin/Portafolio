using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Dtos
{
    public class PortfolioProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? RepoUrl { get; set; }
        public string? DemoUrl { get; set; }
    }
}
