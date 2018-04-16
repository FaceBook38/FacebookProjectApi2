using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FacebookConsumer.Models.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        [Required]
        public string user_email { get; set; }
        [MinLength(8)]
        [MaxLength(12)]
        [Display(Name = "Password")]
        [Required]
        public string user_password { get; set; }
    }
}