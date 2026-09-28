using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraHub.Data.Entities;
using LibraHub.Helpers;

namespace LibraHub.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Reader");

            var user = await _userHelper.GetUserByEmailAsync("admin@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Jovana",
                    LastName = "Matos",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                var result = await _userHelper.AddUserAsync(
                    user,
                    "123456"
                );
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new InvalidOperationException(
                            $"{error.Code}: {error.Description}"
                        );
                    }
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Genres.Any())
            {
                AddGenre("Romance", user);
                AddGenre("Ficção Científica", user);
                AddGenre("Mistério", user);
                AddGenre("Fantasia", user);
                AddGenre("História", user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Authors.Any())
            {
                AddAuthor("José", "Saramago", user);
                AddAuthor("Fernando", "Pessoa", user);
                AddAuthor("Eça", "de Queirós", user);
                AddAuthor("Sophia", "de Mello", user);
                AddAuthor("Camilo", "Castelo Branco", user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Books.Any())
            {
                var genres = _context.Genres.ToList();
                var authors = _context.Authors.ToList();

                AddBook("Memorial do Convento", authors[0], genres[0], 5, "~/images/books/58cfe4c5-a5b5-410c-9d8d-694da6ff6069.jpg", user);
                AddBook("Mensagem", authors[1], genres[3], 3, "~/images/books/9dfe5dd1-ba72-4bf0-8f81-7e504de355fa.jpg", user);
                AddBook("Os Maias", authors[2], genres[0], 4, "~/images/books/3dc3bba4-e821-45c0-ab72-7b0581ef7916.jpg", user);
                AddBook("O Dia dos Prodígios", authors[3], genres[1], 2, "~/images/books/586d1c0f-f079-443b-9148-0714daf968f0.jpg", user);
                AddBook("Amor de Perdição", authors[4], genres[0], 6, "~/images/books/899df580-48f3-46da-8276-d1d9517d3c40.jpg", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddGenre(string name, User user)
        {
            _context.Genres.Add(new Genre
            {
                Name = name,
                User = user
            });
        }

        private void AddAuthor(string firstName, string lastName, User user)
        {
            _context.Authors.Add(new Author
            {
                FirstName = firstName,
                LastName = lastName,
                User = user
            });
        }

        private void AddBook(string title, Author author, Genre genre, int stock, string imageUrl, User user)
        {
            _context.Books.Add(new Book
            {
                Title = title,
                AuthorId = author.Id,
                GenreId = genre.Id,
                Stock = stock,
                ImageUrl = imageUrl,
                User = user,
                Author = author,
                Genre = genre
            });
        }
    }
}
