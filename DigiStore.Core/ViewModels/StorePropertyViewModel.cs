using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Core.ViewModels
{
    public class StorePropertyViewModel
    {
        [Display(Name = "نام فروشگاه")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		[Required(ErrorMessage = "نباید بدون مقدار باشد")]       
        public string Name { get; set; }


        [Display(Name = "آدرس فروشگاه")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public string Address { get; set; }


        [Display(Name = "شماره تماس فروشگاه")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public string Tel { get; set; }


        [Display(Name = "فایل لوگو")]
        public IFormFile? LogoImg { get; set; }

        
        public string? LogoName { get; set; }


        [Display(Name = "سایر توضیحات")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
    }
}
