using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DigiStore.DataAccessLayer.Entities
{
    public class Slider
    {
        
		[Key]        
		public int Id { get; set; }

        	
		[Display(Name = "عنوان")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string Title { get; set; }
        		

		[Display(Name = "تصویر اسلاید")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string Image { get; set; }
		

		[Display(Name = "عدم نمایش")]
		public bool NotShow { get; set; }
		

		[Display(Name = "ترتیب نمایش")]
		public int OrderShow { get; set; }		
		

		[Display(Name = "سایر توضیحات")]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }



    }
}
