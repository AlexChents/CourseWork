using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required]
        public string WeekdaysTimeWork { get; set; }

        [Required]
        public string WeekdaysWork { get; set; }

        [Required]
        public string SaturdayTimeWork { get; set; }

        [Required]
        public string SaturdayWork { get; set; }

        [Required]
        public string SundayTimeWork { get; set; }

        [Required]
        public string SundayWork { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public Schedule()
        {
            Departments = new List<Department>();
        }
    }
}