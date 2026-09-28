using System.Collections.Generic;
using System.Threading.Tasks;
using LibraHub.Data.Entities;

namespace LibraHub.Data
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        IEnumerable<Book> GetAllWithAuthorsGenresAndUsers();

        Task<Book> GetByIdWithRelatedAsync(int id);
    }
}
