namespace Portafolio.Application.Interfaces.Persistence;

public interface IPortfolioProfileRepository
{
    Task<PortfolioProfile?> GetByIdWithProjectsAsync(Guid id, bool asNoTracking = true);
    Task<PortfolioProfile?> GetByIdForUpdateAsync(Guid id);
    Task AddAsync(PortfolioProfile profile);
    void Update(PortfolioProfile profile);
}
