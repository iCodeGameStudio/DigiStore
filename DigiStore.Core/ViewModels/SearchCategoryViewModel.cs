using DigiStore.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Core.ViewModels
{
    public class SearchCategoryViewModel
    {
        public List<Product> FillProducts { get; set; }

        public List<Category> FillCategories { get; set; }

        public Category FillParentCategory { get; set; }

        public Category FillSelectCategory { get; set; }

        public List<Store> FillStores { get; set; }
    }
}
