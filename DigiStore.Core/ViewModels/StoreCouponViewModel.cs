using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Core.ViewModels
{
    public class StoreCouponViewModel
    {

        [Display(Name = "نام کوپن تخفیف")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]       
		[Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public string Name { get; set; }



        [Display(Name = "کد کوپن تخفیف")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public string Code { get; set; }



        [Display(Name = "سایر توضیحات")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }



        [Display(Name = "درصد تخفیف")]
        public int Percent { get; set; }



        [Display(Name = "مبلغ تخفیف")]
        public int Price { get; set; }


        [Display(Name = "تاریخ شروع")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string StartDate { get; set; }

        // 1404/01/01
        [Display(Name = "تاریخ پایان")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string EndDate { get; set; }


        [Display(Name = "منقضی شده")]
        public bool IsExpire { get; set; }

    }
}
