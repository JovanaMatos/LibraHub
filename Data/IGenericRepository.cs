using System.Collections.Generic;
using System.Threading.Tasks;
using LibraHub.Data.Entities;

namespace LibraHub.Data
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        bool ExistAsync(int id);
    }
}
