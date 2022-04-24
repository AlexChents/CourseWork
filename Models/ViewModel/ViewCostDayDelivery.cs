using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models.ViewModel
{
    public class ViewCostDayDelivery
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("RegionSend")]
        [Display(Name = "Область")]
        public int RegionSendId { get; set; }

        public virtual Region RegionSend { get; set; }

        [Required]
        [Display(Name = "Город")]
        [ForeignKey("CitySend")]
        public int CitySendId { get; set; }

        public virtual City CitySend { get; set; }

        [Required]
        [Display(Name = "Область")]
        [ForeignKey("RegionRecipient")]
        public int RegionRecipientId { get; set; }

        public virtual Region RegionRecipient { get; set; }

        [Required]
        [Display(Name = "Город")]
        [ForeignKey("CityRecipient")]
        public int CityRecipientId { get; set; }

        public virtual City CityRecipient { get; set; }

        [Required]
        [Display(Name = "Вес")]
        public double Weight { get; set; }

    }
}