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
    }
}
