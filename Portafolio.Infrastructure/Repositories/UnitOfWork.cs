using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    private readonly Dictionary<Type, object> _repos = new();

    public UnitOfWork(ApplicationDbContext db) => _db = db;

    public IRepository<T> Repo<T>() where T : class
    {
        var type = typeof(T);

        if (!_repos.TryGetValue(type, out var repo))
        {
            repo = new Repository<T>(_db);
            _repos.Add(type, repo);
        }

        return (IRepository<T>)repo;
    }

    public Task<int> SaveAsync() => _db.SaveChangesAsync();
    public void Dispose() => _db.Dispose();
}
