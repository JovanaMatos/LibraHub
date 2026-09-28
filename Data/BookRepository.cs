using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraHub.Data
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllWithAuthorsGenresAndUsers()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.User)
                .OrderBy(b => b.Title)
                .AsNoTracking();
        }

        public async Task<Book> GetByIdWithRelatedAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
