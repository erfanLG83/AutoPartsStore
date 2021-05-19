using System.ComponentModel.DataAnnotations;

namespace AutoPartsStore.Infrastructure.ViewModels.Account
{
    public class RegisterModel
    {

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string LastName { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string Email { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        [Compare("Password", ErrorMessage = "کلمه عبور وارد شده با تکرار کلمه عبور مطابقت ندارد.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [StringLength(4, ErrorMessage = "کد امنیتی باید دارای 4 کاراکتر باشد.")]
        [Display(Name = "کد امنیتی")]
        public string CaptchaCode { get; set; }
    }
}
