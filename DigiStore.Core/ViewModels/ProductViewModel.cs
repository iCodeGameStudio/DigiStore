using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace DigiStore.Core.ViewModels
{
    public class ProductViewModel
    {
        public int BrandId { get; set; }
        public int CategoryId { get; set; }


        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }


        [Display(Name = "تصویر شاخص")]
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }


        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "لطفا فقط عدد وارد کنید.")]
        public int Price { get; set; }


        [Display(Name = "قیمت قبل")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "لطفا فقط عدد وارد کنید.")]
        public int DeletePrice { get; set; }


        [Display(Name = "موجودی")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "لطفا فقط عدد وارد کنید.")]
        public int Exist { get; set; }


        [Display(Name = "عدم نمایش")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public bool NotShow { get; set; }


        [Display(Name = "سایر توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}
