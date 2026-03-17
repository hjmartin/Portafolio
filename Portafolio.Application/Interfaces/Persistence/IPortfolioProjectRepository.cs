namespace Portafolio.Application.Interfaces.Persistence;

public interface IPortfolioProjectRepository
{
    Task<IReadOnlyList<PortfolioProject>> GetByProfileIdAsync(Guid profileId);
    Task<PortfolioProject?> GetByIdAsync(Guid id, bool includeTechnologies = false, bool asNoTracking = true);
    Task AddAsync(PortfolioProject project);
    void Update(PortfolioProject project);
    void Remove(PortfolioProject project);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> HasTechnologyAsync(Guid projectId, Guid technologyId);
    Task AddTechnologyAsync(Guid projectId, Guid technologyId);
    Task RemoveTechnologyAsync(Guid projectId, Guid technologyId);
}
