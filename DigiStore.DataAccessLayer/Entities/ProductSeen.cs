using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DigiStore.DataAccessLayer.Entities
{
    public class ProductSeen
    {
        
		[Key]
		public int Id { get; set; }
		
		public int ProductId { get; set; }

        [Display(Name = "تاریخ")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]		
		public string Date { get; set; }

        [Display(Name = "ساعت")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Time { get; set; }

        [Display(Name = "IP")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string IP { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}
