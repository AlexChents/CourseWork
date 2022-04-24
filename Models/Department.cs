using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class Department
    {
        [Key]  
        [Display(Name ="Номер отделения")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("City")]
        public int CityId { get; set; }

        public virtual City City { get; set; }

        [Required]
        [Display(Name = "Адрес отделения")]
        public string Adress { get; set; }

        [Required]
        [Display(Name = "Максимальный вес")]
        public int MaxWeight { get; set; }

        [Required]
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }

        [Display(Name = "График работы")]
        public virtual Schedule Schedule { get; set; }
        
        [Required]
        [ForeignKey("StatusDepartment")]
        [Display(Name = "Статус отделения")]
        public int StatusDepartmentId { get; set; }

        public virtual StatusDepartment StatusDepartment { get; set; }

        public virtual ICollection<Package> Packages { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Recipient> Recipients { get; set; }

        public Department()
        {
            Packages = new List<Package>();
            Users = new List<User>();
            Recipients = new List<Recipient>();
        }
    }
}