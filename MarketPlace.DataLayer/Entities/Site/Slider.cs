using MarketPlace.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Entities.Site
{
    public class Slider : BaseEntity
    {
        [Display(Name = "نام اصلی")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string MainHeader { get; set; }

        [Display(Name = "نام فرعی")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SecondHeader { get; set; }

        [Display(Name = "نام تصویر")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "لینک")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Link { get; set; }

        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }


    }
}
