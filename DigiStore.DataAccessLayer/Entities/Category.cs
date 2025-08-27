using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiStore.DataAccessLayer.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        
		[Display(Name = "نام دسته")]
		[MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
		[Required(ErrorMessage = "نباید بدون مقدار باشد")]
		public string Name { get; set; }


        [Display(Name = "آیکون")]
        [MaxLength(20, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Icon { get; set; }


        [ForeignKey("Parent")]
		public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<StoreCategory> StoreCategories { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<FieldCategory> FieldCategories { get; set; }


    }
}
