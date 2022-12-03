using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.EntitiesExtention
{
    public static class SliderExtention
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return PathExtention.SliderOrigin + slider.ImageName;
        }
    }
}
