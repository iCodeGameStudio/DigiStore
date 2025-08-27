using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DigiStore.Core.ViewModels
{
    public class AdminBannerViewModel
    {
        [Display(Name = "نام جایگاه")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }


        [Display(Name = "سایز جایگاه")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Size { get; set; }



        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "مبلغ اجاره")]
        public int Price { get; set; }


        [Display(Name = "تعداد روز")]
        public int Day { get; set; }


        [Display(Name = "تصویر پیشفرض")]
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }
    }
}
