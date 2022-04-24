using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseChentsov.Models
{
    
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}