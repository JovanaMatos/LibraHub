using System.ComponentModel.DataAnnotations;

namespace LibraHub.Data.Entities
{
    public class Genre : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "O campo {0} pode conter {1} caracteres.")]
        public string Name { get; set; }
    }
}
