using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class StatusDepartment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Состояние отделения")]
        public string StatusName { get; set; }
    }
}