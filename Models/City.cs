using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Город")]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public City()
        {
            Departments = new List<Department>();
        }
    }
}