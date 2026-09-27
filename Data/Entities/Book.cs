using System.ComponentModel.DataAnnotations;

namespace LibraHub.Data.Entities
{
    public class Book : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "O campo {0} pode conter {1} caracteres.")]
        public string Title { get; set; }

        [MaxLength(20, ErrorMessage = "O campo {0} pode conter {1} caracteres.")]
        public string ISBN { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        public int Stock { get; set; }

        public User User { get; set; }
    }
}
