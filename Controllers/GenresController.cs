using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraHub.Data;
using LibraHub.Data.Entities;
using LibraHub.Helpers;
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

            await _repository.DeleteAsync(genre);
            return RedirectToAction(nameof(Index));
        }
    }
}
