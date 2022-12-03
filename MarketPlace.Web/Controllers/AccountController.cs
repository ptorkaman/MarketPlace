using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPlace.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constructor
        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISmsService _smsService;
        public AccountController(IUserService userService, ICaptchaValidator captchaValidator, ISmsService smsService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _smsService = smsService;
        }

        #endregion

        #region register
        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نیست";
                return View(register);
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.MobileExists:
                        TempData[ErrorMessage] = "تلفن همراه وارد شده تکراری میباشد.";
                        ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری میباشد");
                        break;
                    case RegisterUserResult.Succcess:
                        TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد.";
                        TempData[InfoMessage] = "کد فعال سازی برای شما ارسال گردید.";
                        return RedirectToAction("ActivateMobile","Account",new { mobile=register.Mobile});
                }


            }
            return View(register);
        }

        #endregion

        #region login
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(loginUser.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نیست";
                return View(loginUser);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.GetUserForLogin(loginUser);
                switch (res)
                {
                    case LoginUserResults.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد.";
                        break;
                    case LoginUserResults.NotActivated:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده است";
                        break;
                    case LoginUserResults.Success:
                        var user = await _userService.GetUserByMobile(loginUser.Mobile);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.Mobile),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = loginUser.RememberMe
                        };

                        await HttpContext.SignInAsync(principal, properties);

                        TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد.";
                        return Redirect("/");
                }
            }
            return View();
        }
        #endregion

        #region activateMobile

        [HttpGet("activate-mobile/{mobile}")]
        public IActionResult ActivateMobile(string mobile)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activateMobileDto = new ActivateMobileDTO { Mobile = mobile };
            return View(activateMobileDto);
        }

        [HttpPost("activate-mobile/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateMobile(ActivateMobileDTO activate)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(activate.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نیست";
                return View(activate);
            }
            if (ModelState.IsValid)
            {
                var res =await _userService.ActivateMobile(activate);
                if (res)
                {
                    TempData[SuccessMessage] = "فعالسازی حساب کاربری با موفقیت انجام شد.";
                    return RedirectToAction("Login");
                }
            }
            return View(activate);
        }


        #endregion

        #region forgot password
        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost("forgot-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نیست";
                return View(forgot);
            }
            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordResults.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case ForgotPasswordResults.Success:
                        TempData[ErrorMessage] = "کلمه عبور جدید برای شما ارسال شد.";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری کلمه عبور خود را تغییر دهید";
                        return RedirectToAction("Login");


                }
                    

            }
            return View(forgot);
        }
        #endregion

        #region Log Out
        [HttpGet("log-out")]
        public async Task<IActionResult>  LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion
    }
}
