using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class User
    {
        [Key]
        [ForeignKey("UserLogin")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public virtual UserLogin UserLogin { get; set; }

        [Required]
        [Display(Name = "Отправитель")]
        public string SenderName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string ThirdName { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        //[RegularExpression(@"[0 - 9]{10}", ErrorMessage = "Введите данные в указанном формате")]
        public string PhoneNumber { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Package> Packages { get; set; }

        public virtual ICollection<DispatchRegister> DispatchRegisters { get; set; }

        public User()
        {
            Packages = new List<Package>();
            DispatchRegisters = new List<DispatchRegister>();
        }

    }
}