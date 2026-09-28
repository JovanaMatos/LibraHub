using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraHub.Data.Entities;

namespace LibraHub.Models
{
    public class BookViewModel : Book
    {
        [Display(Name = "Imagem")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> Authors { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; }
    }
}
