using Microsoft.EntityFrameworkCore;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Infrastructure.Persistence;

namespace Portafolio.Infrastructure.Repositories;

public sealed class PortfolioProjectRepository : IPortfolioProjectRepository
{
    private readonly ApplicationDbContext _db;

    public PortfolioProjectRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PortfolioProject>> GetByProfileIdAsync(Guid profileId)
    {
        return await _db.Projects
            .AsNoTracking()
            .Include(x => x.Technologies)
                .ThenInclude(x => x.Technology)
            .Where(x => x.ProfileId == profileId)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Title)
            .ToListAsync();
    }

    public async Task<PortfolioProject?> GetByIdAsync(Guid id, bool includeTechnologies = false, bool asNoTracking = true)
    {
        IQueryable<PortfolioProject> query = _db.Projects;

        if (includeTechnologies)
        {
            query = query
                .Include(x => x.Technologies)
                    .ThenInclude(x => x.Technology);
        }

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task AddAsync(PortfolioProject project)
        => _db.Projects.AddAsync(project).AsTask();

    public void Update(PortfolioProject project)
        => _db.Projects.Update(project);

    public void Remove(PortfolioProject project)
        => _db.Projects.Remove(project);

    public Task<bool> ExistsByIdAsync(Guid id)
        => _db.Projects.AnyAsync(x => x.Id == id);

    public Task<bool> HasTechnologyAsync(Guid projectId, Guid technologyId)
        => _db.ProjectTechnologies.AnyAsync(x => x.ProjectId == projectId && x.TechnologyId == technologyId);

    public Task AddTechnologyAsync(Guid projectId, Guid technologyId)
        => _db.ProjectTechnologies.AddAsync(ProjectTechnology.Create(projectId, technologyId)).AsTask();

    public async Task RemoveTechnologyAsync(Guid projectId, Guid technologyId)
    {
        var relation = await _db.ProjectTechnologies
            .FirstAsync(x => x.ProjectId == projectId && x.TechnologyId == technologyId);

        _db.ProjectTechnologies.Remove(relation);
    }
}
