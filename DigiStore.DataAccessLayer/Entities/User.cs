using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.DataAccessLayer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(11, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }



        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }



        [Display(Name = "کد ملی")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Code { get; set; }


        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        [Display(Name = "نام و نام خانوادگی")]
        public string? FullName { get; set; }



        [Display(Name = "کد فعالسازی")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(6, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string ActiveCode { get; set; }



        [Display(Name = "فعال")]
        public bool IsActive { get; set; }


        [Display(Name = "تاریخ عضویت")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(10, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Date { get; set; }

        public virtual Role Role { get; set; }

        public virtual Store Store { get; set; }
    }
}
