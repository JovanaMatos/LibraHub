using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraHub.Data;
using LibraHub.Data.Entities;
using LibraHub.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenresController : Controller
    {
        private readonly IGenreRepository _repository;
        private readonly IUserHelper _userHelper;

        public GenresController(IGenreRepository repository, IUserHelper userHelper)
        {
            _repository = repository;
            _userHelper = userHelper;
        }

        // GET: Genres
        public IActionResult Index()
        {
            return View(_repository.GetAllWithUsers());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _repository.GetByIdAsync(id.Value);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                genre.User = await _userHelper.GetUserByEmailAsync(
                    this.User.Identity.Name);

                await _repository.CreateAsync(genre);
                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _repository.GetByIdAsync(id.Value);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                genre.User = await _userHelper.GetUserByEmailAsync(
                    this.User.Identity.Name);

                await _repository.UpdateAsync(genre);
                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _repository.GetByIdAsync(id.Value);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _repository.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            try
            {
                await _repository.DeleteAsync(genre);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{genre.Name} provavelmente está a ser usado.";
                    ViewBag.ErrorMessage = $"{genre.Name} não pode ser apagado porque existem livros que o utilizam.<br/>" +
                        $"Experimente primeiro apagar todos os livros que o estão a usar e torne novamente a apagá-lo.";
                }

                return View("Error");
            }
        }
    }
}
