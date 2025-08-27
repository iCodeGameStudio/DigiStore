using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Core.ViewModels
{
    public class UpdateStoreCategoryViewModel
    {
        [Display(Name = "تأیید")]
        public bool IsActive { get; set; }

        [Display(Name = "سایر توضیحات")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}
