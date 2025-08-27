using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DigiStore.Core.ViewModels
{
    public class StoreBankViewModel
    {

        [Display(Name = "شماره شبا")]
        [MaxLength(24, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [MinLength(24, ErrorMessage = "مقدار {0} نباید کمتر از {1} کاراکتر باشد")]      
		[Phone(ErrorMessage = "لطفا شماره شبا معتبر وارد نمایید")]        
		[Required(ErrorMessage = "لطفا شماره شبا معتبر وارد نمایید")]
        public string? BankCard { get; set; }
    }
}
