using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models.ViewModel
{
    public class ViewPackage
    {
        public int Id { get; set; }

        [Display(Name = "Отправитель")]
        [Required]
        public string SenderName { get; set; }

        [Required]
        public string LastNameSender { get; set; }
        [Required]
        public string FirstNameSender { get; set; }
        [Required]
        public string ThirdNameSender { get; set; }

        [Display(Name = "Номер телефона")]
        [Required]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Введите данные в указанном формате")]
        public string NumberPhoneSender { get; set; }

        [Display(Name = "Область")]
        [Required]
        public int RegionSender { get; set; }

        [Display(Name = "Город")]
        [Required]
        public int CitySender { get; set; }

        [Display(Name = "Номер отделения")]
        [Required]
        public string DepartmentIdSender { get; set; }

        [Display(Name = "Получатель")]
        [Required]
        public string RecipientName { get; set; }

        [Display(Name = "Фаимиля")]
        [Required]
        public string LastNameRecipient { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string FirstNameRecipient { get; set; }
        public string ThirdNameRecipient { get; set; }

        [Display(Name = "Номер телефона")]
        [Required]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Введите данные в указанном формате")]
        public string NumberPhoneRecipient { get; set; }

        [Display(Name = "Область")]
        [Required]
        public int RegionRecipient { get; set; }

        [Display(Name = "Город")]
        [Required]
        public int CityRecipient { get; set; }

        [Display(Name = "Номер отделения")]
        [Required]
        public string DepartmentIdRecipient { get; set; }

        [Display(Name = "Вес посылки")]
        [Required]
        [Range(0.1, 1000)]
        public double Weight { get; set; }

        [Display(Name = "Количество мест")]
        [Required]
        [Range(typeof(int), "1", "99")]
        public int CountSeats { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

    }
}