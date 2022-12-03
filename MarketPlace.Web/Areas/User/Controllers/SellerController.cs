using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.Web.PresentationExtentions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region
        private readonly ISellerService _sellerServise;
        public SellerController(ISellerService sellerService)
        {
            _sellerServise = sellerService;
        }
        #endregion


        #region seller panel
        [HttpGet("request-seller-panel")]
        public async Task<IActionResult> RequestSellerPanel()
        {
            return View();
        }


        [HttpPost("request-seller-panel")]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO seller)
        {
            var selleReqResult = await _sellerServise.AddNewSellerRequest(seller, User.GetUserId());

            switch (selleReqResult)
            {
                case RequestSellerResult.HasNotPermision:
                    TempData[ErrorMessage]="شما دسترسی لازم جهت انجام فرایند مورد نظر را ندارید";
                    break;
                case RequestSellerResult.HasUnderProgress:
                    TempData[WarningMessage] = "درخواست های قبلی شما در حال پیگیری میباشند.";
                    TempData[InfoMessage] = "در حال حاضر نمیتوانید درخواست جدیدی ثبت کنید.";
                    break;
                case RequestSellerResult.Success:
                    TempData[SuccessMessage] = "درخواست شما با موقیت ثبت شد.";
                    TempData[InfoMessage] = "فرایند تایید اطلاعات شما در حال پیگیری میباشد.";
                    return RedirectToAction("SellerRequests");
                    break;
            }

            return View(seller);
        }
        #endregion

        #region seller requests
        [HttpGet("seller-requests")]
        public async Task<IActionResult>  SellerRequests(FilterSellerDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.TakeEntity = 5;
            return View(await _sellerServise.FilterSellers(filter));
        }

        #endregion

        #region edit request seller
        [HttpGet("edit-request-seller")]
        public async Task<IActionResult> EditRequestSeller(long Id)
        {
            var requestseller = await _sellerServise.GetRequestSellerForEdit(Id, User.GetUserId());
            if (requestseller == null) return NotFound();
            return View(requestseller);
        }

        [HttpPost("edit-request-seller"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequestSeller(EditRequestSellerDTO request)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerServise.EditRequestSeller(request, User.GetUserId());

                switch (res)
                {
                    case EditRequestResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد.";
                        break;
                    case EditRequestResult.success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ویرایش شد.";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات از سر گرفته شد.";
                        return RedirectToAction("SellerRequests");
                }
            }
            
            return View(request);
        }
        #endregion
    }
}
