using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Classes;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;
using System.Globalization;
using DigiStore.Core.ViewModels;

namespace DigiStore.Controllers
{
    public class HomeController : Controller
    {
        private ITemp _temp;
        private PersianCalendar pc = new PersianCalendar();

        public HomeController(ITemp temp)
        {
            _temp = temp;
        }

        public IActionResult SearchByCategory(int id)
        {
            if(id != 0)
            {
                List<Product> products = _temp.GetProducts(id);
                Category category = _temp.GetCategory(id);
                List<Category> categories = _temp.GetCategoryById(Convert.ToInt32(category.ParentId));
                Category parentCategory = _temp.GetCategory(Convert.ToInt32(category.ParentId));
                List<Store> stores = _temp.GetStores(id);
                var viewModel = new SearchCategoryViewModel();
                viewModel.FillCategories = categories;
                viewModel.FillProducts = products;
                viewModel.FillParentCategory = parentCategory;
                viewModel.FillSelectCategory = category;
                viewModel.FillStores = stores;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //  [RoleAttribute(9)]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            List<BannerDetails> bannerDetails = _temp.GetBannerDetailsNoExpire();
            if(bannerDetails.Count() > 0)
            {
                string strToday = pc.GetYear(DateTime.Now).ToString("0000") + "/" +
                      pc.GetMonth(DateTime.Now).ToString("00") + "/" +
                      pc.GetDayOfMonth(DateTime.Now).ToString("00");

                foreach (var item in bannerDetails)
                {
                    if(item.EndDate.CompareTo(strToday) < 0)
                    {
                        _temp.UpdateBannerExpire(item.Id);
                    }
                }
            }
            return View();
        }

        public IActionResult Banners()
        {
            List<Banner> banners = _temp.GetBanners();
            return View(banners);
        }

    }
}
