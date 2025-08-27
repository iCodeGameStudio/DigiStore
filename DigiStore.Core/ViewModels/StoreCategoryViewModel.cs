using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Core.ViewModels
{
    public class StoreCategoryViewModel
    {
        public int CategoryId { get; set; }



        [Display(Name = "فایل لوگو")]
        public IFormFile? Img { get; set; }


        public string? ImgName { get; set; }

    }
}
