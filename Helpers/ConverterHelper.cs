using LibraHub.Data.Entities;
using LibraHub.Models;

namespace LibraHub.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Author ToAuthor(AuthorViewModel model, string path, bool isNew)
        {
            return new Author
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = path,
                User = model.User
            };
        }

        public AuthorViewModel ToAuthorViewModel(Author author)
        {
            return new AuthorViewModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                ImageUrl = author.ImageUrl,
                User = author.User
            };
        }

        public Book ToBook(BookViewModel model, string path, bool isNew)
        {
            return new Book
            {
                Id = isNew ? 0 : model.Id,
                Title = model.Title,
                AuthorId = model.AuthorId,
                GenreId = model.GenreId,
                ImageUrl = path,
                Stock = model.Stock,
                User = model.User
            };
        }

        public BookViewModel ToBookViewModel(Book book)
        {
            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                GenreId = book.GenreId,
                ImageUrl = book.ImageUrl,
                Stock = book.Stock,
                User = book.User
            };
        }
    }
}
