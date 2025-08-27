using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.DataAccessLayer.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "سطح دسترسی")]
        [Required(ErrorMessage = "نباید بدون مقدار باشد")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}
