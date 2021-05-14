using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoPartsStore.Infrastructure.ViewModels.Account
{
    public class LoginModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string UserName { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public LoginModel()
        {

        }
        public LoginModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
    }
}
