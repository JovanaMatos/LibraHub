using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LibraHub.Data
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Author> GetAllWithUsers()
        {
            return _context.Authors
                .Include(a => a.User)
                .OrderBy(a => a.LastName)
                .AsNoTracking();
        }
    }
}
