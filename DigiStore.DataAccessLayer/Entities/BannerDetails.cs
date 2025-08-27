using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DigiStore.DataAccessLayer.Entities
{
    public class BannerDetails
    {
        
		[Key]

		public int Id { get; set; }

		public int BannerId { get; set; }
		

		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		[Display(Name = "عنوان")]
		public string Title { get; set; }		
		

		[Display(Name = "تصویر بنر")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string Image { get; set; }	
		

		[Display(Name = "تاریخ شروع")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string StartDate { get; set; }	
		

		[Display(Name = "تاریخ پایان")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string EndDate { get; set; }


        [Display(Name = "لینک سایت")]
        public string Url { get; set; }


        [Display(Name = "منقضی")]
		public bool IsExpire { get; set; }

		
		[ForeignKey("BannerId")]
		public virtual Banner Banner { get; set; }

    }
}
