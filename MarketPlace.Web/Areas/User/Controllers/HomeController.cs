using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
    }
}
