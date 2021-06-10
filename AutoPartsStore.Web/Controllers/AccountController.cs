using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Infrastructure.ViewModels.Account;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppUserManager _userManager;
        private readonly IEmailSender _emailSender;
        public AccountController(SignInManager<AppUser> signInManager, IAppUserManager userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        #region Login
        [Route("login")]
        [Route("Account/Login")]
        public IActionResult Login(string returnUrl = "/") => View(new LoginModel(returnUrl));
        [Route("login")]
        [Route("Account/Login")]
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
                else if (!user.EmailConfirmed)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var href = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host + HttpContext.Request.PathBase}/account/confirmemail?email={user.Email}&token={token}";
                    await _emailSender.SendEmailAsync(user.Email, "تایید ایمیل - اتوایران",
                        @$"
                            <p>برای تایید ایمیل خود <a href='{href}'>کلیک کنید</a .<p>
                        ");
                    ModelState.AddModelError("", "ابتدا ایمیل خود را تایید کنید.ایمیل خود را برای تایید چک کنید");
                }

            }
            return View(loginModel);
        }
        #endregion
        #region Register
        public IActionResult Register() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (Captcha.ValidateCaptchaCode(model.CaptchaCode, HttpContext))
                {
                    var user = new AppUser
                    {
                        Email = model.Email,
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        // remove if send email confirm
                        EmailConfirmed = true
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        /* در حال حاضر بدلیل مشکلاتی ایمیل تایید ارسال نمیشود ! */
                        //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var href = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host + HttpContext.Request.PathBase}/account/confirmemail?email={user.Email}&token={token}";
                        //await _emailSender.SendEmailAsync(user.Email, "تایید ایمیل - اتوایران",
                        //    @$"
                        //    <p>برای تایید ایمیل خود <a href='{href}'>کلیک کنید</a .<p>
                        //");
                        await _signInManager.SignInAsync(user, true);
                        return LocalRedirect("/");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                }
                else
                    ModelState.AddModelError("", "کد امنیتی اشتباه است");
            }
            return View(model);
        }
        #endregion
        #region AccessDenied
        public IActionResult AccessDenied(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public async Task<IActionResult> Relogin(string returnUrl = "/")
        {
            var user = await _userManager.FindByIdAsync(User.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await _signInManager.RefreshSignInAsync(user);
            return LocalRedirect(returnUrl);
        }
        #endregion
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || token == null)
                return NotFound();
            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            return LocalRedirect("/");
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/");
        }
        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }
}