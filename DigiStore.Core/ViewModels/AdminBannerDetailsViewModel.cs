using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Core.ViewModels
{
    public class AdminBannerDetailsViewModel
    {
        [Display(Name = "عنوان")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }


        [Display(Name = "لینک سایت")]
        [Url(ErrorMessage = "لطفا یک آدرس سایت معتبر وارد کنید.")]
        public string Url { get; set; }


        [Display(Name = "تصویر بنر")]
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }
    }
}
