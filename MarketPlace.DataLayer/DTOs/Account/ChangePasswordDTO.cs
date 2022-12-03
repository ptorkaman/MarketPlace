using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Account
{
    public class ChangePasswordDTO
    {
        [Display(Name = "رمزعبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string CurrentPassword { get; set; }

        [Display(Name = "رمزعبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمزعبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ComfirmNewPassword { get; set; }
    }
}
