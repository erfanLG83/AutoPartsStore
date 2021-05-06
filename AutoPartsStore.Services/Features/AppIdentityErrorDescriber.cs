using AutoPartsStore.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoPartsStore.Services.Features
{
    public class AppIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) =>
            new IdentityError { Code = nameof(DuplicateUserName), Description = $"نام کاربری '{userName}' توسط شخص دیگری استفاده شده است . " };
        public override IdentityError PasswordRequiresDigit() =>
            new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "کلمه عبور باید حداقل شامل یک عدد (0-9) باشد"
            };
        public override IdentityError InvalidEmail(string email) =>
        new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = "ایمیل شما به درستی وارد نشده",
        };
        public override IdentityError InvalidUserName(string userName) =>
        new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = $"'{userName}' صحیح نمی باشد . لطفا از حروف لاتین و اعداد و علامت های مانند (@ , # , - , _ , .) استفاده کنید"
        };
        public override IdentityError PasswordTooShort(int length) =>
        new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = $"تعداد حداقل کرکترهای کلمه عبور باید {length} باشد"
        };
        public override IdentityError DuplicateEmail(string email) =>
        new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = $"شما با ایمیل {email} قبلا ثبت نام کرده اید . میتوانید وارد شوید"
        };
        public override IdentityError DuplicateRoleName(string role) =>
            new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = $"نقش {role} قبلا ثبت شده است"
            };
        public override IdentityError UserAlreadyInRole(string role) =>
             new IdentityError
             {
                 Code = nameof(UserAlreadyInRole),
                 Description = $"کاربر در نقش ${role} هست!"
             };
    }
}
