using Microsoft.EntityFrameworkCore;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Infrastructure.Persistence;

namespace Portafolio.Infrastructure.Repositories;

public sealed class TechnologyRepository : ITechnologyRepository
{
    private readonly ApplicationDbContext _db;

    public TechnologyRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Technology>> GetAllAsync()
    {
        return await _db.Technologies
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Technology?> GetByIdAsync(Guid id, bool asNoTracking = true)
    {
        IQueryable<Technology> query = _db.Technologies;
        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Technology?> GetByNameAsync(string name, bool asNoTracking = true)
    {
        var normalized = name.Trim();

        IQueryable<Technology> query = _db.Technologies;
        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower());
    }

    public Task AddAsync(Technology technology)
        => _db.Technologies.AddAsync(technology).AsTask();

    public void Update(Technology technology)
        => _db.Technologies.Update(technology);

    public void Remove(Technology technology)
        => _db.Technologies.Remove(technology);

    public Task<bool> ExistsByIdAsync(Guid id)
        => _db.Technologies.AnyAsync(x => x.Id == id);
}
