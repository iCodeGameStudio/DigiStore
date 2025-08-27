using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace DigiStore.Core.ViewModels
{
    public class AdminSliderViewModel
    {
        [Display(Name = "عنوان")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }


        [Display(Name = "ترتیب نمایش")]
        public int OrderShow { get; set; }


        [Display(Name = "سایر توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "تصویر اسلاید")]
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }


        [Display(Name = "عدم نمایش")]
        public bool NotShow { get; set; }
    }
}
