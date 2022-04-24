using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата")]
        public DateTime DateCreate { get; set; }

        [Required]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Содержание")]
        public string Content { get; set; }
    }
}