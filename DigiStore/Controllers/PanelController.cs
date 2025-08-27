using DigiStore.Core.Classes;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.Core.ViewModels;
using DigiStore.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Permissions;

namespace DigiStore.Controllers
{

    [Authorize]
    public class PanelController : Controller
    {
        int productId = 0;
        private IUser _user;
        private IStore _store;
        private PersianCalendar pc = new PersianCalendar();
        public PanelController(IUser user, IStore store)
        {
            _user = user;
            _store = store;
        }
        public IActionResult Dashboard()
        {
            //  Store store = _store.GetStore(_user.GetUserStore(User.Identity.Name).UserId);

            return View();
        }

        private bool CheckStoreUser()
        {
            string username = User.Identity.Name;
            string roleName = _user.GetUserRoleName(username);
            if (roleName == "فروشگاه")
            {
                return true;
            }
            {
                return false;
            }
        }

        #region StoreActivate

        public IActionResult Activate()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                string username = User.Identity.Name;
                string roleName = _user.GetUserRoleName(username);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Activate(StoreActivateViewModel viewModel)
        {
            // ۱. بررسی اعتبارسنجی سمت سرور
            if (!ModelState.IsValid)
            {
                TempData["ErrorTitle"] = "اعتبارسنجی ناموفق";
                TempData["ErrorMessage"] = "اطلاعات وارد شده معتبر نیست. لطفاً دوباره بررسی کنید.";
                return RedirectToAction("Activate", "Panel");
            }

            string username = User.Identity.Name;
            Store store = _user.GetUserStore(username);

            // ۲. بررسی کدهای فعال‌سازی
            if (!_user.ExistsMobileActivate(username, viewModel.MobileCode))
            {
                TempData["ErrorTitle"] = "کد نامعتبر";
                TempData["ErrorMessage"] = "کد فعالسازی موبایل شما معتبر نمی‌باشد.";
                return RedirectToAction("Activate", "Panel");
            }

            if (!_user.ExistsMailActivate(username, viewModel.MailCode))
            {
                TempData["ErrorTitle"] = "کد نامعتبر";
                TempData["ErrorMessage"] = "کد فعالسازی ایمیل شما معتبر نمی‌باشد.";
                return RedirectToAction("Activate", "Panel");
            }

            // ۳. فعال‌سازی حساب کاربری
            try
            {
                _user.ActiveMobileNumber(username);
                _user.ActiveMailAddress(store.Mail);

                TempData["SuccessTitle"] = "فعال‌سازی موفق";
                TempData["SuccessMessage"] = "حساب کاربری شما با موفقیت فعال شد.";
            }
            catch (Exception ex)
            {
                TempData["ErrorTitle"] = "خطای فعال‌سازی";
                TempData["ErrorMessage"] = $"مشکلی در فعال‌سازی حساب پیش آمد. {ex.Message}";
            }

            return RedirectToAction("Activate", "Panel");
        }

        #endregion

        #region StoreEdit

        public IActionResult Edit()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                Store store = _store.GetStore(_user.GetUserStore(User.Identity.Name).UserId);
                StorePropertyViewModel viewModel = new StorePropertyViewModel()
                {
                    Address = store.Address,
                    Description = store.Description,
                    LogoName = store.logo,
                    Name = store.Name,
                    Tel = store.Tel
                };
                //  ViewBag.MyStatus = false;
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult Edit(StorePropertyViewModel viewModel)
        {
            // 1. بررسی اعتبارسنجی سمت سرور
            if (!ModelState.IsValid)
            {
                TempData["ErrorTitle"] = "اعتبارسنجی ناموفق";
                TempData["ErrorMessage"] = "برخی فیلدها به درستی تکمیل نشده‌اند. لطفاً دوباره بررسی کنید.";
                return RedirectToAction("Edit", "Panel");
            }

            // 2. بازیابی اطلاعات فروشگاه
            Store store = _store.GetStore(_user.GetUserStore(User.Identity.Name).UserId);
            if (store == null)
            {
                TempData["ErrorTitle"] = "عدم یافتن فروشگاه";
                TempData["ErrorMessage"] = "فروشگاهی برای ویرایش پیدا نشد.";
                return RedirectToAction("Edit", "Panel");
            }

            string logoName = store.logo;

            // 3. آپلود لوگو
            if (viewModel.LogoImg != null)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/webp" };

                string extension = Path.GetExtension(viewModel.LogoImg.FileName).ToLower();
                string contentType = viewModel.LogoImg.ContentType.ToLower();

                if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                {
                    TempData["ErrorTitle"] = "فرمت نامعتبر";
                    TempData["ErrorMessage"] = "تنها فرمت‌های تصویری JPG, JPEG, PNG, GIF, WEBP مجاز هستند.";
                    return RedirectToAction("Edit", "Panel");
                }

                // حذف فایل قبلی
                if (!string.IsNullOrEmpty(store.logo))
                {
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Users/Stores/", store.logo);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                logoName = CodeGenerators.FileCode() + extension;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Users/Stores/", logoName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.LogoImg.CopyTo(stream);
                    }
                }
                catch
                {
                    TempData["ErrorTitle"] = "خطا در ذخیره‌سازی";
                    TempData["ErrorMessage"] = "مشکلی در ذخیره فایل لوگو پیش آمد. لطفاً دوباره تلاش کنید.";
                    return RedirectToAction("Edit", "Panel");
                }
            }

            // 4. بروزرسانی
            bool isUpdate = _store.UpdateStore(store.UserId, viewModel.Name, viewModel.Tel, viewModel.Address, viewModel.Description, logoName);

            if (isUpdate)
            {
                TempData["SuccessTitle"] = "ویرایش موفق";
                TempData["SuccessMessage"] = "مشخصات فروشگاه با موفقیت بروزرسانی شد.";
            }
            else
            {
                TempData["ErrorTitle"] = "خطای بروزرسانی";
                TempData["ErrorMessage"] = "مشکلی در بروزرسانی مشخصات فروشگاه پیش آمد.";
            }

            return RedirectToAction("Edit", "Panel");
        }

        public IActionResult BankCard()
        {
            string username = User.Identity.Name;
            Store store = _user.GetUserStore(username);

            StoreBankViewModel viewModel = new StoreBankViewModel()
            {
                BankCard = store.BankCard
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult BankCard(StoreBankViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                Store store = _user.GetUserStore(username);
                _store.UpdateBankCard(store.UserId, viewModel.BankCard);
                //return Json(new { success = true, redirectUrl = "/Panel/ShowCoupons" });
                TempData["SuccessTitle"] = "عملیات موفقیت آمیز";
                TempData["SuccessMessage"] = "شماره شبا با موفقیت ثبت گردید.";
                return View(viewModel);
            }
            TempData["ErrorTitle"] = "خطای بروزرسانی";
            TempData["ErrorMessage"] = "مشکلی در ثبت شماره شبا پیش آمده است.";
            return View(viewModel);
        }

        #endregion

        #region StoreCategories

        public IActionResult ShowStoreCategory()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                Store store = _store.GetStore(_user.GetUserStore(User.Identity.Name).UserId);
                List<StoreCategory> storeCategories = _store.GetStoreCategoriesByStoreId(store.UserId);
                return View(storeCategories);
            }
        }

        public IActionResult AddStoreCategory()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewBag.CategoryId = new SelectList(_store.GetCategoriesByNullParent(), "Id", "Name");
                return PartialView("AddStoreCategory", new StoreCategoryViewModel());
            }
        }

        [HttpPost]
        public IActionResult AddStoreCategory(StoreCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var store = _store.GetStore(_user.GetUserStore(User.Identity.Name).UserId);

                string imgName = null;

                // لیست پسوندهای مجاز
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                // لیست MIME Type های مجاز
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

                if (viewModel.Img != null)
                {
                    string extension = Path.GetExtension(viewModel.Img.FileName).ToLower();
                    string contentType = viewModel.Img.ContentType.ToLower();

                    // بررسی پسوند و MIME
                    if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                    {
                        ModelState.AddModelError("Img", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");

                        // دوباره SelectList رو پر کن تا dropdown کار کنه
                        ViewBag.CategoryId = new SelectList(
                            _store.GetCategoriesByNullParent(), "Id", "Name", viewModel.CategoryId
                        );

                        return PartialView("AddStoreCategory", viewModel);
                    }

                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Users/Stores/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Img.CopyTo(stream);
                    }
                }

                var storeCategory = new StoreCategory
                {
                    CategoryId = viewModel.CategoryId,
                    Date = $"{pc.GetYear(DateTime.Now):0000}/{pc.GetMonth(DateTime.Now):00}/{pc.GetDayOfMonth(DateTime.Now):00}",
                    Time = $"{pc.GetHour(DateTime.Now):00}:{pc.GetMinute(DateTime.Now):00}:{pc.GetSecond(DateTime.Now):00}",
                    Image = imgName,
                    UserId = store.UserId
                };
                _store.AddStoreCategory(storeCategory);

                return Json(new { success = true, redirectUrl = "/Panel/ShowStoreCategory" });
            }

            // اگه ModelState.Invalid باشه هم باید دوباره ViewBag پر بشه
            ViewBag.CategoryId = new SelectList(
                _store.GetCategoriesByNullParent(), "Id", "Name", viewModel.CategoryId
            );

            return PartialView("AddStoreCategory", viewModel);
        }


        public IActionResult EditStoreCategory(int id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var storeCategory = _store.GetStoreCategory(id);
                ViewBag.CategoryId = new SelectList(_store.GetCategoriesByNullParent(), "Id", "Name");

                var viewModel = new StoreCategoryViewModel
                {
                    CategoryId = storeCategory.CategoryId,
                    ImgName = storeCategory.Image
                };

                return PartialView("EditStoreCategory", viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditStoreCategory(StoreCategoryViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var storeCategory = _store.GetStoreCategory(id);
                string imgName = storeCategory.Image;

                // لیست پسوندهای مجاز
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                // لیست MIME Type های مجاز
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

                if (viewModel.Img != null)
                {
                    string extension = Path.GetExtension(viewModel.Img.FileName).ToLower();
                    string contentType = viewModel.Img.ContentType.ToLower();

                    // بررسی پسوند و MIME
                    if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                    {
                        ModelState.AddModelError("Img", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");
                        return PartialView("EditStoreCategory", viewModel);
                    }

                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Users/Stores/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Img.CopyTo(stream);
                    }
                }


                _store.UpdateStoreCategory(id, viewModel.CategoryId, imgName);
                return Json(new { success = true, redirectUrl = "/Panel/ShowStoreCategory" });
            }

            return PartialView("EditStoreCategory", viewModel);
        }



        public IActionResult DetailsStoreCategory(int id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                StoreCategory storeCategory = _store.GetStoreCategory(id);
                return View(storeCategory);
            }
        }


        public IActionResult DeleteStoreCategory(int? id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {

                if (id == null)
                    return BadRequest();

                var storeCategory = _store.GetStoreCategory(id.Value);
                if (storeCategory == null)
                    return NotFound();

                return PartialView("DeleteStoreCategory", storeCategory);
            }
        }

        [HttpPost]
        public IActionResult DeleteStoreCategory(int id)
        {
            _store.DeleteStoreCategory(id);
            return Json(new { success = true, redirectUrl = "/Panel/ShowStoreCategory" });
        }

        #endregion

        #region Brands

        public IActionResult ShowBrands()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                string username = User.Identity.Name;
                Store store = _user.GetUserStore(username);
                List<Brand> brands = _store.GetBrands(store.UserId);
                return View(brands);
            }
        }

        public IActionResult AddBrand()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddBrand(AdminBrandViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                Store store = _user.GetUserStore(username);
                if (!_store.ExistsBrand(viewModel.Name))
                {
                    string imgName = null;

                    // لیست پسوندهای مجاز
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                    // لیست MIME Type های مجاز
                    string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

                    if (viewModel.Image != null)
                    {
                        string extension = Path.GetExtension(viewModel.Image.FileName).ToLower();
                        string contentType = viewModel.Image.ContentType.ToLower();

                        // بررسی پسوند و MIME
                        if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                        {
                            ModelState.AddModelError("Image", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");
                            return PartialView("AddBrand", viewModel);
                        }

                        // تولید نام جدید فایل
                        imgName = CodeGenerators.FileCode() + extension;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brands/", imgName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            viewModel.Image.CopyTo(stream);
                        }
                        // ثبت برند
                        Brand brand = new Brand()
                        {
                            Image = imgName,
                            Name = viewModel.Name,
                            NotShow = true,
                            StoreId = store.UserId
                        };
                        _store.AddBrand(brand);
                        return Json(new { success = true, redirectUrl = "/Panel/ShowBrands" });
                    }
                    else
                    {
                        // ثبت برند
                        Brand brand = new Brand()
                        {
                            Image = null,
                            Name = viewModel.Name,
                            NotShow = true,
                            StoreId = store.UserId
                        };
                        _store.AddBrand(brand);
                        return Json(new { success = true, redirectUrl = "/Panel/ShowBrands" });
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "این نام برند قبلا ثبت شده است");
                }
            }
            return PartialView("AddBrand", viewModel);
        }

        #endregion

        #region Products

        public IActionResult ShowProducts()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                string username = User.Identity.Name;
                Store store = _user.GetUserStore(username);
                List<Product> products = _store.GetProducts(store.UserId);
                return View(products);
            }

        }

        public IActionResult DetailsProduct(int id)
        {
            Product product = _store.GetProduct(id);
            return View(product);
        }

        public IActionResult AddProduct()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                Store store = _user.GetUserStore(User.Identity.Name);
                var allCategories = _store.GetAllActivatedCategoriesForUser(store.UserId);
                ViewBag.CategoryId = new SelectList(allCategories, "Id", "Name");
                ViewBag.BrandId = new SelectList(_store.AllBrands(), "Id", "Name");
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel viewModel)
        {
            string username = User.Identity.Name;
            Store store = _user.GetUserStore(username);
            if (ModelState.IsValid)
            {
                string imgName = null;

                // لیست پسوندهای مجاز
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                // لیست MIME Type های مجاز
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

                if (viewModel.Image != null)
                {
                    string extension = Path.GetExtension(viewModel.Image.FileName).ToLower();
                    string contentType = viewModel.Image.ContentType.ToLower();

                    // بررسی پسوند و MIME
                    if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                    {
                        ModelState.AddModelError("Image", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");
                        return PartialView("AddProduct", viewModel);
                    }

                    // تولید نام جدید فایل
                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Products/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }
                    // ثبت محصول
                    Product product = new Product()
                    {
                        Image = imgName,
                        BrandId = viewModel.BrandId,
                        CategoryId = viewModel.CategoryId,
                        Date = $"{pc.GetYear(DateTime.Now):0000}/{pc.GetMonth(DateTime.Now):00}/{pc.GetDayOfMonth(DateTime.Now):00}",
                        Time = $"{pc.GetHour(DateTime.Now):00}:{pc.GetMinute(DateTime.Now):00}:{pc.GetSecond(DateTime.Now):00}",
                        Price = viewModel.Price,
                        DeletePrice = viewModel.DeletePrice,
                        Exist = viewModel.Exist,
                        Description = viewModel.Description,
                        Name = viewModel.Name,
                        NotShow = viewModel.NotShow,
                        StoreId = store.UserId
                    };
                    _store.AddProduct(product);
                    return Json(new { success = true, redirectUrl = "/Panel/ShowProducts" });
                }

            }
            var allCategories = _store.GetAllActivatedCategoriesForUser(store.UserId);
            ViewBag.CategoryId = new SelectList(allCategories, "Id", "Name", viewModel.CategoryId);
            ViewBag.BrandId = new SelectList(_store.AllBrands(), "Id", "Name", viewModel.BrandId);
            return PartialView("AddProduct", viewModel);
        }


        public IActionResult EditProduct(int id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {

                Store store = _user.GetUserStore(User.Identity.Name);

                var allCategories = _store.GetAllActivatedCategoriesForUser(store.UserId);
                ViewBag.CategoryId = new SelectList(allCategories, "Id", "Name");
                ViewBag.BrandId = new SelectList(_store.AllBrands(), "Id", "Name");

                Product product = _store.GetProduct(id);

                ProductViewModel viewModel = new ProductViewModel()
                {
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
                    DeletePrice = product.DeletePrice,
                    Price = product.Price,
                    Exist = product.Exist,
                    Description = product.Description,
                    NotShow = product.NotShow,
                    ImageName = product.Image,
                    Name = product.Name
                };
                return PartialView("EditProduct", viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditProduct(ProductViewModel viewModel, int id)
        {
            string username = User.Identity.Name;
            Store store = _user.GetUserStore(username);
            if (ModelState.IsValid)
            {
                Product product = _store.GetProduct(id);
                string imgName = null;

                // لیست پسوندهای مجاز
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                // لیست MIME Type های مجاز
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

                if (viewModel.Image != null)
                {
                    string extension = Path.GetExtension(viewModel.Image.FileName).ToLower();
                    string contentType = viewModel.Image.ContentType.ToLower();

                    // بررسی پسوند و MIME
                    if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                    {
                        ModelState.AddModelError("Image", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");
                        return PartialView("EditProduct", viewModel);
                    }

                    // تولید نام جدید فایل
                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Products/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }
                    // ویرایش محصول

                    _store.UpdateProduct(id, viewModel.BrandId, viewModel.CategoryId, viewModel.Name, imgName, viewModel.Price, viewModel.DeletePrice, viewModel.Exist, viewModel.NotShow, viewModel.Description);

                    return Json(new { success = true, redirectUrl = "/Panel/ShowProducts" });
                }
                else
                {
                    _store.UpdateProduct(id, viewModel.BrandId, viewModel.CategoryId, viewModel.Name, product.Image, viewModel.Price, viewModel.DeletePrice, viewModel.Exist, viewModel.NotShow, viewModel.Description);

                    return Json(new { success = true, redirectUrl = "/Panel/ShowProducts" });
                }

            }
            var allCategories = _store.GetAllActivatedCategoriesForUser(store.UserId);
            ViewBag.CategoryId = new SelectList(allCategories, "Id", "Name", viewModel.CategoryId);
            ViewBag.BrandId = new SelectList(_store.AllBrands(), "Id", "Name", viewModel.BrandId);
            return PartialView("EditProduct", viewModel);
        }


        public IActionResult DeleteProduct(int? id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var product = _store.GetProduct(id.Value);
                return PartialView("DeleteProduct", product);
            }
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            if (ModelState.IsValid)
            {
                _store.DeleteProduct(id);
                return Json(new { success = true, redirectUrl = "/Panel/ShowProducts" });
            }
            return View();
        }

        #endregion

        #region ProductGalleries

        public IActionResult AddGallery(int id)
        {
            ViewBag.MyID = id;
            return View();
        }

        [HttpPost]
        public IActionResult AddGallery(int id, GalleryViewModel viewModel)
        {
            if (viewModel.Image != null)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/webp" };
                string extension = Path.GetExtension(viewModel.Image.FileName).ToLower();
                string contentType = viewModel.Image.ContentType.ToLower();

                if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                {
                    TempData["ErrorTitle"] = "فرمت فایل نامعتبر";
                    TempData["ErrorMessage"] = "فقط فرمت‌های تصویری مجاز هستند (JPG, JPEG, PNG, GIF, WEBP)";
                    return RedirectToAction("AddGallery", new { id = id });
                }
                try
                {
                    string Imagename = CodeGenerators.FileCode() + extension;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Galleries", Imagename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }

                    ProductGallery productGallery = new ProductGallery()
                    {
                        ProductId = id,
                        Image = Imagename
                    };

                    _store.AddGallery(productGallery);

                    TempData["SuccessTitle"] = "عملیات موفق";
                    TempData["SuccessMessage"] = "عکس با موفقیت ثبت شد.";
                }
                catch (Exception)
                {
                    TempData["ErrorTitle"] = "خطای ذخیره‌سازی";
                    TempData["ErrorMessage"] = "خطایی در ذخیره فایل جدید رخ داده است.";
                }

                return RedirectToAction("AddGallery", new { id = id });
            }

            TempData["ErrorTitle"] = "خطا";
            TempData["ErrorMessage"] = "هیچ فایلی انتخاب نشده است.";
            return RedirectToAction("AddGallery");
        }


        public IActionResult IndexGallery(int id)
        {
            List<ProductGallery> productGalleries = _store.GetProductGalleries(id);
            productId = id;
            return View(productGalleries);
        }

        public IActionResult DeleteGallery(int id)
        {
            ProductGallery productGallery = _store.GetProductGallery(id);
            _store.DeleteGallery(id);
            return Redirect("/Panel/AddGallery/" + productGallery.ProductId);
        }



        [HttpPost]
        [ValidateAntiForgeryToken] // <-- این ویژگی را حتماً داشته باشید
        public async Task<IActionResult> SetCoverImage(int galleryId, int productId)
        {
            if (galleryId <= 0 || productId <= 0)
            {
                return Json(new { success = true, redirectUrl = "/Panel/AddGallery/" + productId });

            }

            await _store.SetCoverImage(galleryId, productId);

            return Json(new { success = true, redirectUrl = "/Panel/AddGallery/" + productId });
        }

        #endregion

        #region ProductFields

        public IActionResult IndexProductFields(int id)
        {
            Product product = _store.GetProduct(id);
            Category category = _store.GetCategory(product.CategoryId);
            int? categoryId = category.Parent.ParentId;
            List<FieldCategory> fieldCategories = _store.GetFieldCategories((int)categoryId);
            ViewBag.MyID = id;
            return View(fieldCategories);
        }

        public IActionResult UpdateProductFields(int id, string result)
        {
            char[] dash = new char[] { '-' };
            string[] strResult = result.Split(dash);
            _store.DeleteAllProductFields(id);

            List<ProductFieldViewModel> models = new List<ProductFieldViewModel>();
            int counter = 1;

            foreach (var item in strResult)
            {
                ProductFieldViewModel productField = new ProductFieldViewModel()
                {
                    Id = counter,
                    Value = item
                };
                models.Add(productField);
                counter++;
            }
            Product product = _store.GetProduct(id);
            Category category = _store.GetCategory(product.CategoryId);
            int? categoryId = category.Parent.ParentId;
            List<FieldCategory> fieldCategories = _store.GetFieldCategories((int)categoryId);

            counter = 1;

            foreach (var item in fieldCategories)
            {
                ProductFieldViewModel viewModel = models.FirstOrDefault(x => x.Id == counter);
                ProductField productField = new ProductField()
                {
                    ProductId = id,
                    Value = viewModel.Value,
                    FieldId = item.FieldId
                };
                _store.AddProductField(productField);
                counter++;
                // return View(productField);
            }
            return Redirect("/Panel/ShowProducts/");
        }

        #endregion

        #region Coupons

        public IActionResult ShowCoupons()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                string username = User.Identity.Name;
                Store store = _user.GetUserStore(username);
                List<Coupon> coupons = _store.GetCoupons(store.UserId);
                return View(coupons);
            }
        }

        public IActionResult AddCoupon()
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                string strToday = pc.GetYear(DateTime.Now).ToString("0000") + "/" + pc.GetMonth(DateTime.Now).ToString("00") + "/" + pc.GetDayOfMonth(DateTime.Now).ToString("00");
                ViewBag.MyDate = strToday;
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddCoupon(StoreCouponViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_store.ExistsCouponCode(viewModel.Code))
                {
                    string username = User.Identity.Name;
                    Store store = _user.GetUserStore(username);
                    Coupon coupon = new Coupon()
                    {
                        Name = viewModel.Name,
                        Code = viewModel.Code,
                        Description = viewModel.Description,
                        EndDate = viewModel.EndDate,
                        StartDate = viewModel.StartDate,
                        IsExpire = false,
                        StoreId = store.UserId,
                        Percent = viewModel.Percent,
                        Price = viewModel.Price
                    };
                    _store.AddCoupon(coupon);
                    return Json(new { success = true, redirectUrl = "/Panel/ShowCoupons" });
                }
                else
                {
                    return PartialView("AddCoupon", viewModel);
                }
            }
            return PartialView("AddCoupon", viewModel);
        }

        public IActionResult EditCoupon(int id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                string strToday = pc.GetYear(DateTime.Now).ToString("0000") + "/" + pc.GetMonth(DateTime.Now).ToString("00") + "/" + pc.GetDayOfMonth(DateTime.Now).ToString("00");
                ViewBag.MyDate = strToday;
                Coupon coupon = _store.GetCoupon(id);

                StoreCouponViewModel viewModel = new StoreCouponViewModel()
                {
                    Code = coupon.Code,
                    Description = coupon.Description,
                    EndDate = coupon.EndDate,
                    StartDate = coupon.StartDate,
                    Name = coupon.Name,
                    Percent = coupon.Percent,
                    Price = coupon.Price
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditCoupon(StoreCouponViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                Coupon coupon = _store.GetCoupon(id);
                _store.UpdateCoupon(id, viewModel.Name, viewModel.Code, viewModel.IsExpire, viewModel.Description, viewModel.StartDate, viewModel.EndDate, viewModel.Percent, viewModel.Price);
                return Json(new { success = true, redirectUrl = "/Panel/ShowCoupons" });
            }
            return PartialView("EditCoupon", viewModel);
        }

        public IActionResult DetailsCoupon(int id)
        {
            Coupon coupon = _store.GetCoupon(id);
            return View(coupon);
        }

        public IActionResult DeleteCoupon(int? id)
        {
            if (!CheckStoreUser())
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var coupon = _store.GetCoupon(id.Value);
                return PartialView("DeleteCoupon", coupon);
            }
        }

        [HttpPost]
        public IActionResult DeleteCoupon(int id)
        {
            if (ModelState.IsValid)
            {
                _store.RemoveCoupon(id);
                return Json(new { success = true, redirectUrl = "/Panel/ShowCoupons" });
            }
            return View();
        }


        #endregion
    }
}