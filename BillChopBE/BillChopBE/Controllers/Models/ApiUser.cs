using System;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Controllers.Models
{
    public class ApiUser
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;
    }

    public class ApiUserWithToken
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}