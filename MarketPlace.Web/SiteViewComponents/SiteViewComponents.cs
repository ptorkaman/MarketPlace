using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.Web.ViewComponents
{
   
    #region Header
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        public SiteHeaderViewComponent(ISiteService siteService,IUserService userService)
        {
            _siteService = siteService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();
            ViewBag.User = null;
            if (User.Identity.IsAuthenticated)
                ViewBag.User = await _userService.GetUserByMobile(User.Identity.Name);
            return View("SiteHeader");
        }
    }
    #endregion

    #region Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();
            return View("SiteFooter");
        }
    }
    #endregion

    #region Slider

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = await _siteService.GetActiveSliders();
            return View("HomeSlider",slider);
        }
    }
    #endregion
}
