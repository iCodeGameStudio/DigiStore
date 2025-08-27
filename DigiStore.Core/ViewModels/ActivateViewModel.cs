using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Core.ViewModels
{
    public class ActivateViewModel
    {
        [Display(Name = "کد فعالسازی")]
        [Phone(ErrorMessage = "لطفا عدد وارد کنید")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(6, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]
        public string ActiveCode { get; set; }
    }
}
