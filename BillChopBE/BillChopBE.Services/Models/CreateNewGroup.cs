using BillChopBE.DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Services.Models
{
    public class CreateNewGroup
    {
        [Required]
        public string Name { get; set; } = null!;

        public Group ToGroup()
        {
            return new Group() { Name = Name };
        }
    }
}
