using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraHub.Data;
using LibraHub.Helpers;
using LibraHub.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public BooksController(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IGenreRepository genreRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View(_bookRepository.GetAllWithAuthorsGenresAndUsers());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetByIdWithRelatedAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            var model = new BookViewModel
            {
                Authors = GetAuthorsSelectList(),
                Genres = GetGenresSelectList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "books");
                }

                var book = _converterHelper.ToBook(model, path, true);

                book.User = await _userHelper.GetUserByEmailAsync(
                    this.User.Identity.Name);

                await _bookRepository.CreateAsync(book);

                return RedirectToAction(nameof(Index));
            }

            model.Authors = GetAuthorsSelectList();
            model.Genres = GetGenresSelectList();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToBookViewModel(book);
            model.Authors = GetAuthorsSelectList();
            model.Genres = GetGenresSelectList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "books");
                    }

                    var book = _converterHelper.ToBook(model, path, false);

                    book.User = await _userHelper.GetUserByEmailAsync(
                        this.User.Identity.Name);

                    await _bookRepository.UpdateAsync(book);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    model.Authors = GetAuthorsSelectList();
                    model.Genres = GetGenresSelectList();
                    return View(model);
                }
            }

            model.Authors = GetAuthorsSelectList();
            model.Genres = GetGenresSelectList();
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetByIdWithRelatedAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            try
            {
                await _bookRepository.DeleteAsync(book);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{book.Title} provavelmente está a ser usado.";
                    ViewBag.ErrorMessage = $"{book.Title} não pode ser apagado porque existem empréstimos que o utilizam.<br/>" +
                        $"Experimente primeiro apagar todos os empréstimos que o estão a usar e torne novamente a apagá-lo.";
                }

                return View("Error");
            }
        }

        private SelectList GetAuthorsSelectList()
        {
            var authors = _authorRepository.GetAll()
                .OrderBy(a => a.LastName);

            return new SelectList(authors, "Id", "FullName");
        }

        private SelectList GetGenresSelectList()
        {
            var genres = _genreRepository.GetAll()
                .OrderBy(g => g.Name);

            return new SelectList(genres, "Id", "Name");
        }
    }
}
