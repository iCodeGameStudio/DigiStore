using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiStore.DataAccessLayer.Entities
{
    public class Brand
    {
        
		[Key]
        public int Id { get; set; }
        
		[Display(Name = "برند")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }


		[Display(Name = "لوگو")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Image { get; set; }


		[Display(Name = "عدم نمایش")]
        public bool NotShow { get; set; }

        public int? StoreId { get; set; }
        
		[ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }
    }
}
