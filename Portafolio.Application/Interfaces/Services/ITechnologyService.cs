using Portafolio.Application.Dtos;

namespace Portafolio.Application.Interfaces.Services;

public interface ITechnologyService
{
    Task<IReadOnlyList<TechnologyDto>> GetAllAsync();
    Task<TechnologyDto> GetByIdAsync(Guid id);
    Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto);
    Task<TechnologyDto> UpdateAsync(Guid id, UpdateTechnologyDto dto);
    Task DeleteAsync(Guid id);
}
