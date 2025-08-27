using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiStore.DataAccessLayer.Entities
{
    public class Store
    {
        [Key]
        [ForeignKey("User")]

        public int UserId { get; set; }


        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Mail { get; set; }


        [Display(Name = "نام فروشگاه")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Name { get; set; }


        [Display(Name = "آدرس فروشگاه")]
        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }


        [Display(Name = "شماره تماس فروشگاه")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Tel { get; set; }

        
        [Display(Name = "فایل لوگو")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? logo { get; set; }


        [Display(Name = "سایر توضیحات")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "فعالسازی موبایل")]
        public bool MobileActivate { get; set; }

        [Display(Name = "فعالسازی ایمیل")]
        public bool MailActivate { get; set; }

        [Display(Name = "کد فعالسازی")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(6, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string MailActivateCode { get; set; }
        
        
        [Display(Name = "موجودی")]
        public long? Wallet { get; set; }


        [Display(Name = "شماره شبا")]
        [MaxLength(24, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? BankCard { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<StoreCategory> StoreCategories { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Coupon> Coupons { get; set; }

    }
}
