using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Entities.Contact
{
    public class ContactUs : BaseEntity
    {
        #region properties
        public long? UserId { get; set; }

        [Display(Name = "آی پی")]
        public string UserIP { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "نام")]
        [MaxLength(200, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "موضوع")]
        [MaxLength(500, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Subject { get; set; }

        [Display(Name = "متن پیام")]
        [MaxLength(2000, ErrorMessage = " {0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string content { get; set; }
        #endregion

        #region relations
        public User User { get; set; }
        #endregion
    }
}
