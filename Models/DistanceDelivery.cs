using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class DistanceDelivery
    {
        public int Id { get; set; }

        [Display(Name = "Первая область")]
        [ForeignKey("RegionFirst")]
        public int FirstRegionId { get; set; }

        public virtual Region RegionFirst { get; set; }

        [Display(Name = "Вторая область")]
        [ForeignKey("RegionSecond")]
        public int SecondRegionId { get; set; }

        public virtual Region RegionSecond { get; set; }

        [ForeignKey("BasicPriceDaysDelivery")]
        [Display(Name = "Тарифная зона")]
        public int BasicPriceDaysDeliveryId { get; set; }

        public virtual BasicPriceDaysDelivery BasicPriceDaysDelivery { get; set; }

    }
}