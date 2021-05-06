using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Auth;
using Microsoft.AspNetCore.Http;

namespace AutoPartsStore.Infrastructure.Admin.UsersManager
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "شیوه ورودی ایمیل صحیح نمی باشد !")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "کد پستی")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} باید کمتر از {2} کارکتر باشد و حداکثر دارای {1} کارکتر باشد", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار کلمه عبور یکسان نمی باشد")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی")]
        [StringLength(50, ErrorMessage = "{0} باید کمتر از {2} کارکتر باشد و حداکثر دارای {1} کارکتر باشد", MinimumLength = 2)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "{0} باید کمتر از {2} کارکتر باشد و حداکثر دارای {1} کارکتر باشد", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [StringLength(20, ErrorMessage = "{0} باید کمتر از {2} کارکتر باشد و حداکثر دارای {1} کارکتر باشد", MinimumLength = 3)]
        public string Username { get; set; }
        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }
        public string UserRoles { get; set; }
        public IFormFile ImageFile { get; set; }
        public AppUser GetUser()
        {
            return new AppUser {
                Email = Email,
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                UserName = Username,
                PhoneNumber = PhoneNumber
                
            };
        }
    }
}
