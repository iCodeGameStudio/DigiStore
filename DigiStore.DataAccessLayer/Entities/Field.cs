using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DigiStore.DataAccessLayer.Entities
{
    public class Field
    {
        
		[Key]
		public int Id { get; set; }

        
		[Display(Name = "نام مشخصه")]
		[MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		public string Name { get; set; }

        public virtual ICollection<FieldCategory> FieldCategories { get; set; }

        public virtual ICollection<ProductField> ProductFields { get; set; }

    }
}
