using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.EntitiesExtention
{
    public static class BannerExtention
    {
        public static string GetBannerMainAddressImage(this SiteBanner banner)
        {
            return PathExtention.SiteBanner + banner.ImageName;
        }
    }
}
