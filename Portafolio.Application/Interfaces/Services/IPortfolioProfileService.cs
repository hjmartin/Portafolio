using Portafolio.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Interfaces.Services
{
    public interface IPortfolioProfileService
    {
        Task<PortfolioProfileDto> CreateAsync(CreatePortfolioProfileDto dto);
        Task<PortfolioProfileDto?> GetByIdAsync(Guid id);
        Task<PortfolioProfileDto> UpdateAsync(Guid id, UpdatePortfolioProfileDto dto);

    }
}
