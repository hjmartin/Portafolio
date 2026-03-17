using Portafolio.Application.Dtos;

namespace Portafolio.Application.Interfaces.Services
{
    public interface IPortfolioProfileService
    {
        Task<PortfolioProfileDto> CreateAsync(CreatePortfolioProfileDto dto);
        Task<PortfolioProfileDto> GetByIdAsync(Guid id);
        Task<PortfolioProfileDto> UpdateAsync(Guid id, UpdatePortfolioProfileDto dto);
    }
}
