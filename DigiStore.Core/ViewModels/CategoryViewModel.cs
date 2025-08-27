using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.Core.ViewModels
{
    public class CategoryViewModel
    {
        
		[Display(Name = "نام دسته")]
		[Required(ErrorMessage = "نباید بدون مقدار باشد")]
		[MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]        		
		public string Name { get; set; }


        [Display(Name = "آیکون")]
        public string Icon { get; set; }

        public bool IsTopLevel { get; set; }
        public List<CategoryViewModel> Children { get; set; } = new List<CategoryViewModel>();
    }
}
