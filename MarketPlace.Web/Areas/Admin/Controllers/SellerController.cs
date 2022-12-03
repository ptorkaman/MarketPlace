using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Common;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.Web.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseController
    {
        #region constructor
        private readonly ISellerService _sellerService;
        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }
        #endregion

        #region seller requests

        [HttpGet("seller-request")]
        public async Task<IActionResult> SellerRequest(FilterSellerDTO filter)
        {
           // filter.TakeEntity = 1;
            return View(await _sellerService.FilterSellers(filter));
        }
        #endregion

        #region accept request
        [HttpGet("accept-request")]
        public async Task<IActionResult> AcceptSellerRequest(long requestId)
        {
           
            var result = await _sellerService.AcceptSellerRequest(requestId);

            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نظر با موفقیت تایید شد",
                    null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد", null);
        }
        #endregion

        #region reject request

        [HttpPost("reject-request"),ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerRequest(RejectItemDTO reject)
        {

            var result = await _sellerService.RejectSellerRequest(reject);

            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نظر با موفقیت تایید شد",
                    reject);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد", null);
        }
        #endregion

    }
}
