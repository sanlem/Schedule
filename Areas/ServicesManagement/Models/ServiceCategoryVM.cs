﻿using Schedule.Areas.ServicesManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Areas.ServicesManagement.Models
{
    public class ServiceCategoryVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Should not be more than 30 symbols!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Should not be more than 100 symbols!")]
        public string Description { get; set; }
        public ServiceCategory? ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
