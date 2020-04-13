using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Areas.ServicesManagement.Data
{
    public class Service
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage ="Should not be more than 30 symbols!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Should not be more than 100 symbols!")]
        public string Description { get; set; }

        [Required]
        public int DefaultDurationMinutes { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public ServiceCategory Category { get; set; }
    }
}
