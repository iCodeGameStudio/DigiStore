using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Core.ViewModels
{
    public class AdminSettingViewModel
    {
        [Display(Name = "نام سایت")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]       
		[Required(ErrorMessage = "وارد کردن نام سایت الزامی می باشد")]

        public string SiteName { get; set; }


        [Display(Name = "توضیحات سایت")]
        [DataType(DataType.MultilineText)]
        public string? SiteDescription { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [DataType(DataType.MultilineText)]
        public string? SiteKeys { get; set; }

        [Display(Name = "API")]
        public string? SmsApi { get; set; }

        [Display(Name = "شماره فرستنده")]
        [MaxLength(15, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? SmsSender { get; set; }
        
		[EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        [Display(Name = "ایمیل فرستنده")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? MailAddress { get; set; }

        
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [Display(Name = "گذرواژه ایمیل")] 
		[DataType(DataType.Password)]
        public string? MailPassword { get; set; }

    }
}
