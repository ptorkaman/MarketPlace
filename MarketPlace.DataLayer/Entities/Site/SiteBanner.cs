﻿using MarketPlace.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Entities.Site
{
    public class SiteBanner : BaseEntity
    {
        [Display(Name = "نام عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "آدرس عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Url { get; set; }

        [Display(Name = "سایز(کلاس های نمایشی)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(30, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ColSize { get; set; }

        public BannerPlacement BannerPlacement { get; set; }

    }
}
