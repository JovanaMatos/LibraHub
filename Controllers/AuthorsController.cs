using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraHub.Data;
using LibraHub.Helpers;
using LibraHub.Models;
using System;
using System.Threading.Tasks;

namespace LibraHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _repository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AuthorsController(
            IAuthorRepository repository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _repository = repository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Authors
        public IActionResult Index()
        {
            return View(_repository.GetAllWithUsers());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _repository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "authors");
                }

                var author = _converterHelper.ToAuthor(model, path, true);

                author.User = await _userHelper.GetUserByEmailAsync(
                    this.User.Identity.Name);

                await _repository.CreateAsync(author);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _repository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToAuthorViewModel(author);
            return View(model);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "authors");
                    }

                    var author = _converterHelper.ToAuthor(model, path, false);

                    author.User = await _userHelper.GetUserByEmailAsync(
                        this.User.Identity.Name);

                    await _repository.UpdateAsync(author);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(model);
                }
            }

            return View(model);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _repository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(author);
            return RedirectToAction(nameof(Index));
        }
    }
}
