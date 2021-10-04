using System;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Models
{
    public class CreateNewBill
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Total { get; set; }

        [Required]
        public Guid LoanerId { get; set; }

        [Required]
        public Guid GroupContextId { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }
    }
}
