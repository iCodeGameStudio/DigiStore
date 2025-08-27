using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Core.ViewModels
{
    public class GalleryViewModel
    {
        [Display(Name = "تصویر گالری")]
        public IFormFile? Image { get; set; }

        public string? ImageName { get; set; }
    }
}
