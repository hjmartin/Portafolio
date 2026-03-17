using Microsoft.EntityFrameworkCore;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Infrastructure.Persistence;

namespace Portafolio.Infrastructure.Repositories;

public sealed class PortfolioProfileRepository : IPortfolioProfileRepository
{
    private readonly ApplicationDbContext _db;

    public PortfolioProfileRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PortfolioProfile?> GetByIdWithProjectsAsync(Guid id, bool asNoTracking = true)
    {
        IQueryable<PortfolioProfile> query = _db.Profiles.Include(x => x.Projects);

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<PortfolioProfile?> GetByIdForUpdateAsync(Guid id)
    {
        return _db.Profiles
            .Include(x => x.Projects)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task AddAsync(PortfolioProfile profile)
        => _db.Profiles.AddAsync(profile).AsTask();

    public void Update(PortfolioProfile profile)
        => _db.Profiles.Update(profile);
}
