using System;
using System.ComponentModel.DataAnnotations;

namespace LibraHub.Models
{
    public class LoanViewModel
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; }

        [Required(ErrorMessage = "O campo Data de Empréstimo é obrigatório.")]
        [Display(Name = "Data de Empréstimo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanDate { get; set; }

        [Required(ErrorMessage = "O campo Data de Devolução Prevista é obrigatório.")]
        [Display(Name = "Data de Devolução Prevista")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Data de Devolução")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDate { get; set; }
    }
}
