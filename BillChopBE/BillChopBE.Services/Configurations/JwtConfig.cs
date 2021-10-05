using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Configurations
{
    public class JwtConfig
    {
        [Required]
        public string Key { get; set; } = null!;

        [Required]
        public string Issuer { get; set; } = null!;

        [Required]
        public string Audience { get; set; } = null!;

        [Required]
        public string Subject { get; set; } = null!;
    }
}