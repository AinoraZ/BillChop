using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer
{
    public class BillChopConfig
    {
        [Required]
        public string BillChopDb { get; set; } = null!;
    }
}
