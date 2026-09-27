using System;
using System.ComponentModel.DataAnnotations;

namespace LibraHub.Data.Entities
{
    public class Loan : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Book")]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public User User { get; set; }

        [Display(Name = "Loan Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LoanDate { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Return Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? ReturnDate { get; set; }
    }
}
