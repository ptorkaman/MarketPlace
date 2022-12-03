﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Seller
{
    public class EditRequestSellerDTO
    {
        public long Id { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string StoreName { get; set; }


        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(12, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Phone { get; set; }

        [Display(Name = "آدرس فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Address { get; set; }
    }
    public enum EditRequestResult
    {
        success,
        NotFound
    }
}
