using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraHub.Data
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly DataContext _context;

        public LoanRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Loan> GetAllWithBooksAndUsers()
        {
            return _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .OrderByDescending(l => l.LoanDate)
                .AsNoTracking();
        }

        public async Task<Loan> GetByIdWithRelatedAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
