using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Core.ViewModels
{
    public class SubCategoriesViewModel
    {
        [Display(Name = "نام زیر دسته")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        public string Name { get; set; }
    }
}
