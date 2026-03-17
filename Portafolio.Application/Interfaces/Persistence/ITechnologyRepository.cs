namespace Portafolio.Application.Interfaces.Persistence;

public interface ITechnologyRepository
{
    Task<IReadOnlyList<Technology>> GetAllAsync();
    Task<Technology?> GetByIdAsync(Guid id, bool asNoTracking = true);
    Task<Technology?> GetByNameAsync(string name, bool asNoTracking = true);
    Task AddAsync(Technology technology);
    void Update(Technology technology);
    void Remove(Technology technology);
    Task<bool> ExistsByIdAsync(Guid id);
}
