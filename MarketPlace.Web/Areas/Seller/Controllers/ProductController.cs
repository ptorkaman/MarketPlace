using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {

        #region constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region list

        [HttpGet("product-list")]
        public async Task<IActionResult> ProductList()
        {
            return View();
        }
        #endregion
    }
}
