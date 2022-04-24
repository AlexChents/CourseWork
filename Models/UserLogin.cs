using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace CourseChentsov.Models
{
    public class UserLogin
    {
        public int Id { get; set; }

        [Display(Name = "Електронная почта")]
        [EmailAddress]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Почта не корректная")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool ConfirmedEmail { get; set; }

        public string Token { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

    }
}