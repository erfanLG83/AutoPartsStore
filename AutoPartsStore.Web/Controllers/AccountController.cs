using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Infrastructure.ViewModels.Account;
using AutoPartsStore.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoPartsStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppUserManager _userManager;
        public AccountController(SignInManager<AppUser> signInManager, IAppUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [Route("login")]
        public IActionResult Login(string returnUrl = "/") => View(new LoginModel(returnUrl));
        [Route("login")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel) 
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user == null)
                    user = await _userManager.FindByEmailAsync(loginModel.UserName);
                if (user != null && user.EmailConfirmed)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, user.LockoutEnabled);
                    if (result.Succeeded)
                        return LocalRedirect(loginModel.ReturnUrl);
                    if (result.IsNotAllowed)
                        ModelState.AddModelError("", "شما اجازه ورود ندارد . بعدا تلاش کنید");
                    if (result.IsLockedOut)
                        ModelState.AddModelError("", "حساب شما قفل شده است بعد از 10 دقیقه دوباره تلاش کنید");
                    if (result.RequiresTwoFactor)
                        ModelState.AddModelError("", "برای ورود نیاز به ورود دو مرحله ای دارید");
                }
                else if (user == null)
                    ModelState.AddModelError("", "کاربری با این نام کاربری یا ایمیل پیدا نشد");
                else if (user.EmailConfirmed)
                    ModelState.AddModelError("", "ابتدا ایمیل خود را تایید کنید !");

            }
            return View(loginModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/");
        }
    }
}