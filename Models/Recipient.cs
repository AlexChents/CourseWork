using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class Recipient
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Получатель")]
        public string RecipientName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string ThirdName { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        //[RegularExpression(@"[0 - 9]{10}", ErrorMessage = "Введите данные в указанном формате")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Електронная почта")]
        [EmailAddress]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Почта не корректная")]
        public string Email { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public Recipient()
        {
            Departments = new List<Department>();
        }
    }
}