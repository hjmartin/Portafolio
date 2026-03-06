using Microsoft.EntityFrameworkCore;
using Portafolio.Application.Interfaces.Persistence;
using Portafolio.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }

    /// Proporciona una consulta de solo lectura (IQueryable) con seguimiento de cambios (Tracking).
    /// Útil cuando se planea modificar las entidades obtenidas más adelante en la misma transacción.
    /// Ejemplo: _repo.Query().Where(x => x.Activo).ToListAsync();
    public IQueryable<T> Query() => _dbSet.AsQueryable();

    /// Proporciona una consulta de solo lectura sin seguimiento de cambios (NoTracking).
    /// Es mucho más rápido y consume menos memoria. Ideal para listados, reportes y solo lectura.
    /// Ejemplo: _repo.QueryNoTracking().Select(x => x.Nombre).ToListAsync();
    public IQueryable<T> QueryNoTracking() => _dbSet.AsNoTracking();

    /// Busca una entidad por su clave primaria (Simple o Compuesta).
    /// Primero busca en la memoria del contexto; si no está, va a la base de datos.
    /// Ejemplo Simple: await _repo.GetByIdAsync(1);
    /// Ejemplo Compuesta: await _repo.GetByIdAsync(sucursalId, productoId);
    public async Task<T?> GetByIdAsync(params object[] ids) => await _dbSet.FindAsync(ids);

    /// Obtiene el primer registro que coincida con el filtro, permitiendo incluir tablas relacionadas de forma tipada.
    /// Ejemplo: await _repo.GetFirstOrDefaultAsync(x => x.Id == id, x => x.Comercio, x => x.Transacciones);
    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> filtro,
        params Expression<Func<T, object>>[] incluir)
    {
        IQueryable<T> query = _dbSet;

        // Agregamos dinámicamente los Includes pasados por el usuario
        foreach (var property in incluir)
        {
            query = query.Include(property);
        }

        return await query.FirstOrDefaultAsync(filtro);
    }

    /// Marca una entidad nueva para ser insertada en la base de datos.
    /// Requiere llamar a SaveAsync() para confirmar la operación.
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    /// Marca un conjunto de entidades nuevas para ser insertadas. Eficiente para inserciones múltiples (Batch).
    public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

    /// Marca una entidad existente como modificada. EF actualizará todos los campos en la base de datos.
    public void Update(T entity) => _dbSet.Update(entity);

    /// Marca una entidad para ser eliminada. 
    /// NOTA: Si se usa el Interceptor de borrado lógico, el estado cambiará a 'Modified' con IsDeleted = true.
    public void Remove(T entity) => _dbSet.Remove(entity);

    /// Marca una colección de entidades para ser eliminadas de forma masiva.
    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    /// Determina de forma rápida si existe algún registro que cumpla con la condición sin descargar los datos.
    /// Ejemplo: if(await _repo.AnyAsync(x => x.Nit == "123")) { ... }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

}
