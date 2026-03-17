using Portafolio.Application.Dtos;

namespace Portafolio.Application.Interfaces.Services;

public interface IPortfolioProjectService
{
    Task<IReadOnlyList<PortfolioProjectDto>> GetByProfileAsync(Guid profileId);
    Task<PortfolioProjectDto> GetByIdAsync(Guid id);
    Task<PortfolioProjectDto> CreateAsync(Guid profileId, CreatePortfolioProjectDto dto);
    Task<PortfolioProjectDto> UpdateAsync(Guid id, UpdatePortfolioProjectDto dto);
    Task DeleteAsync(Guid id);
    Task AddTechnologyAsync(Guid projectId, Guid technologyId);
    Task RemoveTechnologyAsync(Guid projectId, Guid technologyId);
}
