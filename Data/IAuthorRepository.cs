using System.Collections.Generic;
using LibraHub.Data.Entities;

namespace LibraHub.Data
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        IEnumerable<Author> GetAllWithUsers();
    }
}
