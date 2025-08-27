using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;



namespace DigiStore.DataAccessLayer.Entities
{
    public class Banner
    {
        
		[Key]
        public int Id { get; set; }
     
        
		[Display(Name = "نام جایگاه")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Name { get; set; }
      
        
		[Display(Name = "تصویر پیشفرض")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? DefaultImage { get; set; }
        
		
        
        [Display(Name = "سایز جایگاه")]
		[MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Size { get; set; }
     

        
		[Display(Name = "توضیحات")]
		[DataType(DataType.MultilineText)]
        public string? Description { get; set; }
      
        
		[Display(Name = "مبلغ اجاره")]
        public int Price { get; set; }
      
        
		[Display(Name = "تعداد روز")]
        public int Day { get; set; }


        public virtual ICollection<BannerDetails> BannerDetails { get; set; }

    }
}
