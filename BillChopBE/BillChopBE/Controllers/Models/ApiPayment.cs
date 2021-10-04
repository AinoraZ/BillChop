using System;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Controllers.Models
{
    public class ApiPayment
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public virtual ApiUser Payer { get; set; } = null!;

        [Required]
        public virtual ApiUser Receiver { get; set; } = null!;

        [Required]
        public Guid GroupContextId { get; set; }

    }
}