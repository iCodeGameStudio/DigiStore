using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace DigiStore.Core.ViewModels
{
    public class AdminBrandViewModel
    {
        
		[Display(Name = "برند")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public string Name { get; set; }


        [Display(Name = "لوگو")]
        public IFormFile? Image { get; set; }
        
        public string? ImageName { get; set; }

        
		[Display(Name = "عدم نمایش")]
        public bool NotShow { get; set; }
    }
}
