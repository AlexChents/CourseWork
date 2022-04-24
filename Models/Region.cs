using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class Region
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Область")]
        public string RegionName { get; set; }

        public virtual ICollection<City> Cities { get; set; }

            public virtual ICollection<DistanceDelivery> DistanceDeliveries { get; set; }

        public Region()
        {
            DistanceDeliveries = new List<DistanceDelivery>();
            Cities = new List<City>();
        }
    }
}