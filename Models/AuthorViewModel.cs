using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using LibraHub.Data.Entities;

namespace LibraHub.Models
{
    public class AuthorViewModel : Author
    {
        [Display(Name = "Imagem")]
        public IFormFile ImageFile { get; set; }
    }
}
