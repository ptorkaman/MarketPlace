using MarketPlace.DataLayer.DTOs.Site;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.DTOs.Account
{
    public class ForgotPasswordDTO : CaptchaViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(12, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }
    }

    public enum ForgotPasswordResults
    {
        Success,
        Error,
        NotFound
    }
}
