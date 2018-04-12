using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FaceBookAPI.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required]
        public string user_email { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string user_password { get; set; }
    }
}