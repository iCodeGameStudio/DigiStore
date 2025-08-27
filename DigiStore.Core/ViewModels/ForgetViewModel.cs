using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DigiStore.Core.ViewModels
{
    public class ForgetViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(11, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [Phone(ErrorMessage = "فقط مجاز هستید عدد وارد کنید")]
        public string Mobile { get; set; }
    }
}
