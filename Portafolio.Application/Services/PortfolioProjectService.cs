using AutoMapper;
using Portafolio.Application.Common.Exceptions;
using Portafolio.Application.Dtos;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Application.Interfaces.Services;

namespace Portafolio.Application.Services;

public sealed class PortfolioProjectService : IPortfolioProjectService
{
    private readonly IPortfolioProjectRepository _projects;
    private readonly IPortfolioProfileRepository _profiles;
    private readonly ITechnologyRepository _technologies;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PortfolioProjectService(
        IPortfolioProjectRepository projects,
        IPortfolioProfileRepository profiles,
        ITechnologyRepository technologies,
        IUnitOfWork uow,
        IMapper mapper)
    {
        _projects = projects;
        _profiles = profiles;
        _technologies = technologies;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PortfolioProjectDto>> GetByProfileAsync(Guid profileId)
    {
        var profileExists = await _profiles.GetByIdWithProjectsAsync(profileId, asNoTracking: true);
        if (profileExists is null)
            throw new NotFoundException("Profile no existe", "portfolio_profile_not_found");

        var projects = await _projects.GetByProfileIdAsync(profileId);
        return _mapper.Map<IReadOnlyList<PortfolioProjectDto>>(projects);
    }

    public async Task<PortfolioProjectDto> GetByIdAsync(Guid id)
    {
        var project = await _projects.GetByIdAsync(id, includeTechnologies: true, asNoTracking: true);
        if (project is null)
            throw new NotFoundException("Project no existe", "portfolio_project_not_found");

        return _mapper.Map<PortfolioProjectDto>(project);
    }

    public async Task<PortfolioProjectDto> CreateAsync(Guid profileId, CreatePortfolioProjectDto dto)
    {
        var profile = await _profiles.GetByIdWithProjectsAsync(profileId, asNoTracking: true);
        if (profile is null)
            throw new NotFoundException("Profile no existe", "portfolio_profile_not_found");

        var project = PortfolioProject.Create(
            profileId,
            dto.Title,
            dto.Description,
            dto.LiveUrl,
            dto.RepoUrl,
            dto.CoverImageUrl,
            dto.IsFeatured,
            dto.SortOrder);

        await _projects.AddAsync(project);
        await _uow.SaveAsync();

        return _mapper.Map<PortfolioProjectDto>(project);
    }

    public async Task<PortfolioProjectDto> UpdateAsync(Guid id, UpdatePortfolioProjectDto dto)
    {
        var project = await _projects.GetByIdAsync(id, includeTechnologies: true, asNoTracking: false);
        if (project is null)
            throw new NotFoundException("Project no existe", "portfolio_project_not_found");

        project.UpdateDetails(
            dto.Title,
            dto.Description,
            dto.LiveUrl,
            dto.RepoUrl,
            dto.CoverImageUrl,
            dto.IsFeatured,
            dto.SortOrder);

        _projects.Update(project);
        await _uow.SaveAsync();

        return _mapper.Map<PortfolioProjectDto>(project);
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _projects.GetByIdAsync(id, includeTechnologies: false, asNoTracking: false);
        if (project is null)
            throw new NotFoundException("Project no existe", "portfolio_project_not_found");

        _projects.Remove(project);
        await _uow.SaveAsync();
    }

    public async Task AddTechnologyAsync(Guid projectId, Guid technologyId)
    {
        var project = await _projects.GetByIdAsync(projectId, includeTechnologies: true, asNoTracking: false);
        if (project is null)
            throw new NotFoundException("Project no existe", "portfolio_project_not_found");

        var techExists = await _technologies.ExistsByIdAsync(technologyId);
        if (!techExists)
            throw new NotFoundException("Technology no existe", "technology_not_found");

        if (await _projects.HasTechnologyAsync(projectId, technologyId))
            throw new ConflictException("Technology ya está asociada al proyecto", "project_technology_conflict");

        await _projects.AddTechnologyAsync(projectId, technologyId);
        await _uow.SaveAsync();
    }

    public async Task RemoveTechnologyAsync(Guid projectId, Guid technologyId)
    {
        var projectExists = await _projects.ExistsByIdAsync(projectId);
        if (!projectExists)
            throw new NotFoundException("Project no existe", "portfolio_project_not_found");

        var hasTechnology = await _projects.HasTechnologyAsync(projectId, technologyId);
        if (!hasTechnology)
            throw new NotFoundException("La relación proyecto-tecnología no existe", "project_technology_not_found");

        await _projects.RemoveTechnologyAsync(projectId, technologyId);
        await _uow.SaveAsync();
    }
}
