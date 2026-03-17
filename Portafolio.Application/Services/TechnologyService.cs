using AutoMapper;
using Portafolio.Application.Common.Exceptions;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Application.Interfaces.Services;

namespace Portafolio.Application.Services;

public sealed class TechnologyService : ITechnologyService
{
    private readonly ITechnologyRepository _technologies;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public TechnologyService(ITechnologyRepository technologies, IUnitOfWork uow, IMapper mapper)
    {
        _technologies = technologies;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TechnologyDto>> GetAllAsync()
    {
        var items = await _technologies.GetAllAsync();
        return _mapper.Map<IReadOnlyList<TechnologyDto>>(items);
    }

    public async Task<TechnologyDto> GetByIdAsync(Guid id)
    {
        var technology = await _technologies.GetByIdAsync(id, asNoTracking: true);
        if (technology is null)
            throw new NotFoundException("Technology no existe", "technology_not_found");

        return _mapper.Map<TechnologyDto>(technology);
    }

    public async Task<TechnologyDto> CreateAsync(CreateTechnologyDto dto)
    {
        var existing = await _technologies.GetByNameAsync(dto.Name, asNoTracking: true);
        if (existing is not null)
            throw new ConflictException("Ya existe una tecnología con ese nombre", "technology_name_conflict");

        var technology = Technology.Create(dto.Name, dto.IconUrl, dto.Category);
        await _technologies.AddAsync(technology);
        await _uow.SaveAsync();

        return _mapper.Map<TechnologyDto>(technology);
    }

    public async Task<TechnologyDto> UpdateAsync(Guid id, UpdateTechnologyDto dto)
    {
        var technology = await _technologies.GetByIdAsync(id, asNoTracking: false);
        if (technology is null)
            throw new NotFoundException("Technology no existe", "technology_not_found");

        var existing = await _technologies.GetByNameAsync(dto.Name, asNoTracking: true);
        if (existing is not null && existing.Id != id)
            throw new ConflictException("Ya existe una tecnología con ese nombre", "technology_name_conflict");

        technology.UpdateDetails(dto.Name, dto.IconUrl, dto.Category);
        _technologies.Update(technology);
        await _uow.SaveAsync();

        return _mapper.Map<TechnologyDto>(technology);
    }

    public async Task DeleteAsync(Guid id)
    {
        var technology = await _technologies.GetByIdAsync(id, asNoTracking: false);
        if (technology is null)
            throw new NotFoundException("Technology no existe", "technology_not_found");

        _technologies.Remove(technology);
        await _uow.SaveAsync();
    }
}
