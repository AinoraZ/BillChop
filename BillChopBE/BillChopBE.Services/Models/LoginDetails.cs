using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Models
{
    public class LoginDetails
    {
        [Required]
        [RegularExpression(@"^[\w_+-\.]+@([\w-]+\.)+[\w-]{2,}$")]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
