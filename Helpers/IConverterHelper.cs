using LibraHub.Data.Entities;
using LibraHub.Models;

namespace LibraHub.Helpers
{
    public interface IConverterHelper
    {
        Author ToAuthor(AuthorViewModel model, string path, bool isNew);

        AuthorViewModel ToAuthorViewModel(Author author);
    }
}
