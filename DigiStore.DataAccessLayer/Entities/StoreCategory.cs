using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiStore.DataAccessLayer.Entities
{
    public class StoreCategory
    {
        
		[Key]
		public int Id { get; set; }
        

		public int UserId { get; set; }

        
		public int CategoryId { get; set; }

       
		[Display(Name = "تاریخ درخواست")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]		
		public string Date { get; set; }
		
		[Display(Name = "ساعت درخواست")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string Time { get; set; }
		
		[Display(Name = "تأیید")]
		public bool IsActive { get; set; }
		
		[Display(Name = "سایر توضیحات")]		
		public string? Description { get; set; }
		
		[Display(Name = "تصویر مدارک")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]		
		public string? Image { get; set; }

		
		[ForeignKey("UserId")]
		public virtual Store Store { get; set; }
		
		[ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}
