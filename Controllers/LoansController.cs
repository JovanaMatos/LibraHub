using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraHub.Data;
using LibraHub.Data.Entities;
using LibraHub.Helpers;
using LibraHub.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraHub.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserHelper _userHelper;

        public LoansController(
            ILoanRepository loanRepository,
            IBookRepository bookRepository,
            IUserHelper userHelper)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View(_bookRepository.GetAllWithAuthorsGenresAndUsers());
        }

        public IActionResult Create(int? bookId)
        {
            if (bookId == null)
            {
                return NotFound();
            }

            var book = _bookRepository.GetByIdWithRelatedAsync(bookId.Value).Result;

            if (book == null)
            {
                return NotFound();
            }

            var model = new LoanViewModel
            {
                BookId = book.Id,
                BookTitle = book.Title,
                LoanDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(15),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = await _bookRepository.GetByIdAsync(model.BookId);

                if (book == null)
                {
                    return NotFound();
                }

                if (book.Stock <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Este livro não tem stock disponível para empréstimo.");
                    model.BookTitle = book.Title;
                    return View(model);
                }

                var loan = new Loan
                {
                    BookId = model.BookId,
                    LoanDate = model.LoanDate,
                    DueDate = model.DueDate,
                    User = await _userHelper.GetUserByEmailAsync(
                        this.User.Identity.Name)
                };

                await _loanRepository.CreateAsync(loan);

                book.Stock--;
                await _bookRepository.UpdateAsync(book);

                return RedirectToAction(nameof(Index));
            }

            var bookInfo = await _bookRepository.GetByIdAsync(model.BookId);
            model.BookTitle = bookInfo?.Title;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ActiveLoans()
        {
            return View(_loanRepository.GetAllWithBooksAndUsers());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _loanRepository.GetByIdWithRelatedAsync(id.Value);

            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _loanRepository.GetByIdWithRelatedAsync(id.Value);

            if (loan == null)
            {
                return NotFound();
            }

            var model = new LoanViewModel
            {
                Id = loan.Id,
                BookId = loan.BookId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldLoan = await _loanRepository.GetByIdAsync(model.Id);

                    var loan = new Loan
                    {
                        Id = model.Id,
                        BookId = model.BookId,
                        LoanDate = model.LoanDate,
                        DueDate = model.DueDate,
                        ReturnDate = model.ReturnDate,
                        User = await _userHelper.GetUserByEmailAsync(
                            this.User.Identity.Name)
                    };

                    await _loanRepository.UpdateAsync(loan);

                    var book = await _bookRepository.GetByIdAsync(model.BookId);

                    if (oldLoan != null && book != null)
                    {
                        if (!oldLoan.ReturnDate.HasValue && model.ReturnDate.HasValue)
                        {
                            book.Stock++;
                            await _bookRepository.UpdateAsync(book);
                        }
                        else if (oldLoan.ReturnDate.HasValue && !model.ReturnDate.HasValue)
                        {
                            book.Stock--;
                            await _bookRepository.UpdateAsync(book);
                        }
                    }

                    return RedirectToAction(nameof(ActiveLoans));
                }
                catch (Exception)
                {
                    return View(model);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _loanRepository.GetByIdWithRelatedAsync(id.Value);

            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            if (!loan.ReturnDate.HasValue)
            {
                var book = await _bookRepository.GetByIdAsync(loan.BookId);
                if (book != null)
                {
                    book.Stock++;
                    await _bookRepository.UpdateAsync(book);
                }
            }

            await _loanRepository.DeleteAsync(loan);
            return RedirectToAction(nameof(ActiveLoans));
        }
    }
}
