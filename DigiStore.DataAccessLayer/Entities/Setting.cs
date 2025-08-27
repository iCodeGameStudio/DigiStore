using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.DataAccessLayer.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }				

		[Display(Name = "نام سایت")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string SiteName { get; set; }
        

		[Display(Name = "توضیحات سایت")]
		[DataType(DataType.MultilineText)]
        public string? SiteDescription { get; set; }

        
		[Display(Name = "کلمات کلیدی")]
		[DataType(DataType.MultilineText)]
        public string? SiteKeys { get; set; }

        
		[Display(Name = "API")]
        public string? SmsApi { get; set; }

        
		[Display(Name = "شماره فرستنده")]
		[MaxLength(15, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? SmsSender { get; set; }
        

		[Display(Name = "ایمیل فرستنده")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? MailAddress { get; set; }


        
		[Display(Name = "گذرواژه ایمیل")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? MailPassword { get; set; }
    }
}
