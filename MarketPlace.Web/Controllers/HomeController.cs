using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.DataLayer.Entities.Site;
using MarketPlace.Web.Models;
using MarketPlace.Web.PresentationExtentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constructor
        private readonly ISmsService _smsService;
        private readonly IContactService _contactservice;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISiteService _siteService;

        public HomeController(ISmsService smsService, IContactService contactService,ICaptchaValidator captchaValidator,
            ISiteService siteService)
        {
            _smsService = smsService;
            _contactservice = contactService;
            _captchaValidator = captchaValidator;
            _siteService = siteService;
        }
        #endregion

        #region index
        public async Task<IActionResult> Index()
        {
            //await _smsService.SendVerificationSMS();
           
            ViewBag.banner = await _siteService.GetSiteBannersByPlacement(new List<BannerPlacement> {
                    BannerPlacement.Home_1,
                    BannerPlacement.Home_2,
                    BannerPlacement.Home_3
            });

            return View();
        }
        #endregion

        #region contact us
        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            return View();
        }
        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO createContact)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(createContact.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نیست";
                return View(createContact);
            }
            
            if (ModelState.IsValid)
            {
                await _contactservice.ContactUs(createContact, User.GetUserId(), HttpContext.GetUserIP());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("ContactUs");
            }
            return View(createContact);
        }

        #endregion

        #region about-us
        [HttpGet("about-us")]
        public async Task<ActionResult> AboutUs()
        {
            var SiteSetting = await _siteService.GetDefaultSiteSetting();
            return View(SiteSetting);
        }
        #endregion
    }
}
