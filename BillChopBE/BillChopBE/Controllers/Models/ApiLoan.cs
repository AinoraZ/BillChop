using System;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Controllers.Models
{
    public class ApiLoan
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public virtual ApiUser Loanee { get; set; } = null!;
        
        [Required]
        public ApiUser Loaner { get; set; } = null!;

        [Required]
        public Guid BillId { get; set; }
    }
}