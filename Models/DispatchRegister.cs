using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class DispatchRegister
    {
        [Display(Name = "Номер реестра")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата создания")]
        public DateTime DateCreate { get; set; }

        public virtual ICollection<Package> Packages { get; set; }

        [ForeignKey("User")]
        [Display(Name = "отправитель")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        [Display(Name = "Распечатано")]
        [DefaultValue("false")]
        public bool IsPrint { get; set; }

        [Required]
        [Display(Name = "В отделении отправки")]
        [DefaultValue("false")]
        public bool InDepartmentSend { get; set; }

        public DispatchRegister()
        {
            Packages = new List<Package>();
        }
    }
}