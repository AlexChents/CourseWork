using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class StatusPackage
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public string StatusName { get; set; }
    }
}