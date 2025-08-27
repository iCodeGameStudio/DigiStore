using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiStore.DataAccessLayer.Entities
{
    public class ProductGallery
    {
        
		[Key]
		public int Id { get; set; }

		public int ProductId { get; set; }
		
		
		[Display(Name = "تصویر گالری")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string? Image { get; set; }


        [Display(Name = "تصویر کاور")]
        public bool Cover { get; set; }

		
		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }

    }
}
