using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.Entities.Store
{
    public class Seller : BaseEntity
    {
        #region properties
        public long UserId { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string StoreName { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(11, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(12, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Phone { get; set; }

        [Display(Name = "آدرس فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Address { get; set; }

        [Display(Name = " توضیحات فروشگاه")]
        public string Description { get; set; }

        [Display(Name = "یادداشت های ادمین")]
        public string AdminDescription { get; set; }

        [Display(Name = "توضیحات تایید / عدم تایید")]
        public string StoreAcceptanceDescription { get; set; }

        public StoreAcceptanceState StoreAcceptanceState { get; set; }
        #endregion

        #region relations
        public User users { get; set; }
        #endregion


    }
    public enum StoreAcceptanceState
    {
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
