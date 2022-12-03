using MarketPlace.DataLayer.Entities.Common;
using MarketPlace.DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Entities.Products
{
    public class Product : BaseEntity
    {
        #region properties
        public long ProductCategoryId { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "قیمت")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تایید/عدم تایید محصول")]
        public ProductAcceptanceState ProductAcceptanceState { get; set; }

        [Display(Name = "توضیحات تایید/ عیدم تایید محصول")]
        public string ProductAcceptOrRejectDescription { get; set; }
        #endregion


        #region relations
        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public Seller seller { get; set; }

        #endregion
    }

    public enum ProductAcceptanceState
    {
        UnderProgress,
        Accepted,
        Rejected
    }
}
