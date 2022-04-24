using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models.ViewModel
{
    public class ViewDepartment
    {
        public int Id { get; set; }

        [Display(Name = "Город")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Адрес отделения")]
        [Required]
        public string Adress { get; set; }

        [Display(Name = "Максимальный вес")]
        [Required]
        [Range(typeof(int), "5", "1000")]
        public int MaxWeight { get; set; }

        [Display(Name = "Статус отделения")]
        [Required]
        public string StatusDepartment { get; set; }

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

    }
}