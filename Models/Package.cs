using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models
{
    public class Package
    {
        public int Id { get; set; }

        [Display(Name = "Номер декларации")]
        public long NumberDelivery { get; set; }

        [Required]
        [Display(Name = "Отделение отправки")]
        public int DepartmentSendId { get; set; }

        [ForeignKey("DepartmentSendId")]
        public virtual Department DepartmentSend { get; set; }
        
        [Required]
        [Display(Name = "Отделение доставки")]
        public int DepartmentRecipientId { get; set; }

        [ForeignKey("DepartmentRecipientId")]
        public virtual Department DepartmentRecipient { get; set; }

        [Required]
        [Display(Name = "Вес посылки")]
        [Range(0.1, 1000)]
        public double Weight { get; set; }

        [Required]
        [Display(Name = "Количество мест")]
        [Range(typeof(int), "1", "99")]
        public int CountSeats { get; set ; }

        [Required]
        [Display(Name = "Описание посылки")]
        public string Description { get; set; }

        [Display(Name = "Дата отправки")]
        public DateTime DateSend { get; set; }

        [Display(Name = "Планируемая дата доставки")]
        public DateTime DateDelivery { get; set; }

        [Display(Name = "Стоимость доставки")]
        public double DeliveryCost { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int RecipientId { get; set; }

        [ForeignKey("RecipientId")]
        public virtual Recipient Recipient { get; set; }

        public int? DispatchRegisterId { get; set; }

        [ForeignKey("DispatchRegisterId")]
        public virtual DispatchRegister DispatchRegister { get; set; }

        public int? StatusPackageId { get; set; }

        [ForeignKey("StatusPackageId")]
        public virtual StatusPackage StatusPackage { get; set; }

        [Required]
        [Display(Name = "В реестре")]
        [DefaultValue("false")]
        public bool InRegister { get; set; }

        [Required]
        [DefaultValue("false")]
        [Display(Name = "Распечатано")]
        public bool IsPrint { get; set; }
    }
}