using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.Core.ViewModels
{
    public class ProductAdminViewModel
    {
        public Product FillProduct { get; set; }

        public List<ProductGallery> FillGalleries { get; set; }

        public List<ProductField> FillFields { get; set; }



    }
}
