using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiStore.DataAccessLayer.Entities
{
    public class ProductField
    {
        
		[Key]
		public int Id { get; set; }

		public int ProductId { get; set; }

		public int FieldId { get; set; }

		
		[Display(Name = "مقدار مشخصه")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string Value { get; set; }

		
		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }

		
		[ForeignKey("FieldId")]
        public virtual Field Field { get; set; }
    }
}
