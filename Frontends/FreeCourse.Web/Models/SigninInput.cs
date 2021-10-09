using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models
{
    public class SigninInput
    {
        [Required]
        [Display(Name ="Email Adresiniz")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Beni hatırla")]
        public bool IsRemember { get; set; }
    }
}
