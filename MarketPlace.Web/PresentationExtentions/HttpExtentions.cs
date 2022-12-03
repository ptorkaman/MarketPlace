using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.PresentationExtentions
{
    public static class HttpExtentions
    {
        public static string GetUserIP(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
