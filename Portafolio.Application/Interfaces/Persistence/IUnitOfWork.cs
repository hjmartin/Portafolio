using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repo<T>() where T : class;
        Task<int> SaveAsync();

        //Repositorios especificos.

    }
}
