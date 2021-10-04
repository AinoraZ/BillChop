using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.Controllers.Models
{
    public class ApiGroup
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual List<ApiUser> Users { get; set; } = new List<ApiUser>();
    }
}