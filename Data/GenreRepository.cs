using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LibraHub.Data
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetAllWithUsers()
        {
            return _context.Genres
                .Include(g => g.User)
                .OrderBy(g => g.Name)
                .AsNoTracking();
        }
    }
}
