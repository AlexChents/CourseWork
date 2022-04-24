using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseChentsov.Models
{
    public class BasicPriceDaysDelivery
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Базовая цена (до 15 кг.)")]
        public double BasicPrice { get; set; }

        [Required]
        [Display(Name = "Цена за 1 кг. (свыше 15 кг.)")]
        public double PriceForKg { get; set; }

        [Required]
        [Display(Name = "Срок доставки, дн.")]
        public int CountDays { get; set; }

        public virtual ICollection<DistanceDelivery> DistanceDeliveries { get; set; }

        public BasicPriceDaysDelivery()
        {
            DistanceDeliveries = new List<DistanceDelivery>();
        }
    }
}