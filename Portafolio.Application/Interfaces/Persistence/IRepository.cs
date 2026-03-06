using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Interfaces.Persistence;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Proporciona una consulta de solo lectura (IQueryable) con seguimiento de cambios (Tracking).
    /// Útil cuando se planea modificar las entidades obtenidas más adelante en la misma transacción.
    /// Ejemplo: _repo.Query().Where(x => x.Activo).ToListAsync();
    /// </summary>
    IQueryable<T> Query();

    /// <summary>
    /// Proporciona una consulta de solo lectura sin seguimiento de cambios (NoTracking).
    /// Es mucho más rápido y consume menos memoria. Ideal para listados, reportes y solo lectura.
    /// Ejemplo: _repo.QueryNoTracking().Select(x => x.Nombre).ToListAsync();
    /// </summary>
    IQueryable<T> QueryNoTracking();

    /// <summary>
    /// Busca una entidad por su clave primaria (Simple o Compuesta).
    /// Primero busca en la memoria del contexto; si no está, va a la base de datos.
    /// Ejemplo Simple: await _repo.GetByIdAsync(1);
    /// Ejemplo Compuesta: await _repo.GetByIdAsync(sucursalId, productoId);
    /// </summary>
    Task<T?> GetByIdAsync(params object[] ids);

    /// <summary>
    /// Obtiene el primer registro que coincida con el filtro, permitiendo incluir tablas relacionadas de forma tipada.
    /// Ejemplo: await _repo.GetFirstOrDefaultAsync(x => x.Id == id, x => x.Comercio, x => x.Transacciones);
    /// </summary>
    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> filtro,
        params Expression<Func<T, object>>[] incluir);


    /// <summary>
    /// Marca una entidad nueva para ser insertada en la base de datos.
    /// Requiere llamar a SaveAsync() para confirmar la operación.
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Marca un conjunto de entidades nuevas para ser insertadas. Eficiente para inserciones múltiples (Batch).
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Marca una entidad existente como modificada. EF actualizará todos los campos en la base de datos.
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Marca una entidad para ser eliminada. 
    /// NOTA: Si se usa el Interceptor de borrado lógico, el estado cambiará a 'Modified' con IsDeleted = true.
    /// </summary>
    void Remove(T entity);

    /// <summary>
    /// Marca una colección de entidades para ser eliminadas de forma masiva.
    /// </summary>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Determina de forma rápida si existe algún registro que cumpla con la condición sin descargar los datos.
    /// Ejemplo: if(await _repo.AnyAsync(x => x.Nit == "123")) { ... }
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
