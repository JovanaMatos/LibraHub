using System.Collections.Generic;
using System.Threading.Tasks;
using LibraHub.Data.Entities;

namespace LibraHub.Data
{
    public interface ILoanRepository : IGenericRepository<Loan>
    {
        IEnumerable<Loan> GetAllWithBooksAndUsers();

        Task<Loan> GetByIdWithRelatedAsync(int id);
    }
}
