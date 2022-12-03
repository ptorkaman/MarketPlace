using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.DataLayer.Repository;
using MarketPlace.Web.PresentationExtentions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class UserAccountController : UserBaseController
    {
        #region constructor

        private readonly IUserService  _userService;
        public UserAccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region change password

        [HttpGet("change-password")]
        public async Task<IActionResult>  ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            if (ModelState.IsValid)
            {
                var res= await _userService.ChangeUserPassword(changePassword, User.GetUserId());
                if (res)
                {
                    TempData[SuccessMessage] = "رمزعبور شما با موفقیت تغییر کرد.";
                    TempData[InfoMessage] = "لطفا جهت تکمیل فرایند تغییر رمزعبور، مجددا وارد سایت شوید.";
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                else
                {
                    TempData[ErrorMessage] = "لطفا از رمزعبور جدید استفاده کنید";
                    ModelState.AddModelError("NewPassword", "لطفا از رمزعبور جدید استفاده کنید");
                }
            }

            return View(changePassword);
        }
        #endregion

        #region change user profile
        [HttpGet("change-user-profile")]
        public async Task<IActionResult> ChangeUserProfile()
        {
            var profile = await _userService.GetUserProfile(User.GetUserId());
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpPost("change-user-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserProfile(EditUserProfileDTO editUser,IFormFile avatarImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.EditUserProfile(editUser,User.GetUserId(),avatarImage);

                switch (res)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما بلاک شده است.";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما غیرفعال است.";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با این مضخصات یافت نشد.";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = $"{editUser.FirstName} {editUser.LastName} حساب کاربری شما با موفقیت ویرایش شد.";
                        return RedirectToAction("ChangeUserProfile");
                        
                }
            }

            return View(editUser);
        }
        #endregion
    }
}
