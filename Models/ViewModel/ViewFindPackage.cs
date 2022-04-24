using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseChentsov.Models.ViewModel
{
    public class ViewFindPackage
    {
     //   public int Id { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Номер делкfрации дожен составлять 11 цифр")]
        [ForeignKey("Package")]
        [Display(Name = "Номер декларации")]
        public string NumberDelarationId { get; set; }

        public Package Package { get; set; }
    }
}