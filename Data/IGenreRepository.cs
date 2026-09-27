using System.Collections.Generic;
using LibraHub.Data.Entities;

namespace LibraHub.Data
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        IEnumerable<Genre> GetAllWithUsers();
    }
}
