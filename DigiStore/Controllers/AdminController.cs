using DigiStore.Core.Classes;
using DigiStore.Core.Interfaces;
using DigiStore.Core.ViewModels;
using DigiStore.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using X.PagedList;
using X.PagedList.Extensions;


namespace DigiStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IAdmin _admin;
        private IUser _user;
        PersianCalendar pc = new PersianCalendar();
        public AdminController(IAdmin admin, IUser user)
        {
            _admin = admin;
            _user = user;
        }

        private bool CheckAdminUser()
        {
            string username = User.Identity.Name;
            string roleName = _user.GetUserRoleName(username);
            if (roleName == "مدیر")
            {
                return true;
            }
            {
                return false;
            }
        }

        public IActionResult Dashboard()
        {
            if (CheckAdminUser())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }

        public IActionResult ShowActiveStores()
        {
            List<Store> stores = _admin.GetActiveStores();
            return View(stores);
        }

        #region Settings

        public IActionResult Setting()
        {
            if (CheckAdminUser())
            {

                Setting setting = _admin.GetSetting();
                if (setting != null)
                {
                    AdminSettingViewModel viewModel = new AdminSettingViewModel()
                    {
                        MailAddress = setting.MailAddress,
                        MailPassword = setting.MailPassword,
                        SiteDescription = setting.SiteDescription,
                        SiteName = setting.SiteName,
                        SiteKeys = setting.SiteKeys,
                        SmsApi = setting.SmsApi,
                        SmsSender = setting.SmsSender
                    };
                    return View(viewModel);
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }


        [HttpPost]
        public IActionResult Setting(AdminSettingViewModel viewModel)
        {
            // 1. بررسی اعتبارسنجی سمت سرور
            if (!ModelState.IsValid)
            {
                TempData["ErrorTitle"] = "اطلاعات نامعتبر";
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = "اطلاعات وارد شده نامعتبر است. لطفاً فیلدها را بررسی کنید. " + string.Join(" ", errors);
                return RedirectToAction("Setting");
            }

            // 2. انجام عملیات به‌روزرسانی یا درج
            try
            {
                if (_admin.ExistsSetting())
                {
                    _admin.UpdateSetting(
                        viewModel.SiteName,
                        viewModel.SiteDescription,
                        viewModel.SiteKeys,
                        viewModel.SmsApi,
                        viewModel.SmsSender,
                        viewModel.MailAddress,
                        viewModel.MailPassword
                    );
                }
                else
                {
                    Setting setting = new Setting()
                    {
                        MailAddress = viewModel.MailAddress,
                        MailPassword = viewModel.MailPassword,
                        SiteDescription = viewModel.SiteDescription,
                        SiteName = viewModel.SiteName,
                        SiteKeys = viewModel.SiteKeys,
                        SmsApi = viewModel.SmsApi,
                        SmsSender = viewModel.SmsSender
                    };
                    _admin.InsertSetting(setting);
                }

                // 3. پیام موفقیت‌آمیز
                TempData["SuccessTitle"] = "عملیات موفق";
                TempData["SuccessMessage"] = "تنظیمات با موفقیت ذخیره شد.";
                return RedirectToAction("Setting");
            }
            catch (Exception ex)
            {
                // 4. پیام خطا
                TempData["ErrorTitle"] = "خطا در ثبت تنظیمات";
                TempData["ErrorMessage"] = $"خطایی در ثبت تنظیمات رخ داد. {ex.Message}";
                return RedirectToAction("Setting");
            }
        }

        #endregion

        #region Permissions

        public IActionResult Permissions(int? page)
        {
            if (CheckAdminUser())
            {
                ViewBag.MyMessage = false;

                //List<Permission> permissions = _admin.GetPermissions();
                int PageSize = 8;
                var PageNumber = page ?? 1;
                var permissions = _admin.GetPermissions().ToPagedList(PageNumber, PageSize);
                ViewBag.MyModels = permissions;

                if (permissions == null || !permissions.Any())
                {
                    ViewBag.IsPermissionListEmpty = true;
                }
                else
                {
                    ViewBag.IsPermissionListEmpty = false;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }

        public IActionResult AddPermission()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                return PartialView("AddPermission", new PermissionViewModel());
            }
        }

        [HttpPost]
        public IActionResult AddPermission(PermissionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var permission = new Permission
                {
                    Name = viewModel.Name
                };
                _admin.InsertPermission(permission);

                // پاسخ JSON برای موفقیت
                return Json(new { success = true, redirectUrl = "/Admin/Permissions" });

            }

            // در حالت خطا PartialView را برگردان با همان مدل
            return PartialView("AddPermission", viewModel);
        }

        public IActionResult EditPermission(int id)
        {
            if (CheckAdminUser())
            {
                Permission permission = _admin.GetPermission(id);
                PermissionViewModel viewModel = new PermissionViewModel()
                {
                    Name = permission.Name
                };
                return PartialView("EditPermission", viewModel);
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }

        }

        [HttpPost]
        public IActionResult EditPermission(int id, PermissionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _admin.UpdatePermission(id, viewModel.Name);
                return Json(new { success = true, redirectUrl = "/Admin/Permissions" });
            }
            return PartialView("EditPermission", viewModel);
        }


        public IActionResult DeletePermission(int? id)
        {
            if (CheckAdminUser())
            {

                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }

        }
        [HttpPost]
        public IActionResult DeletePermission(int id)
        {
            if (ModelState.IsValid)
            {
                _admin.DeletePermission(id);
                return Json(new { success = true, redirectUrl = "/Admin/Permissions" });
            }
            return View();
        }

        #endregion

        #region Categories

        public IActionResult Categories()
        {
            if (CheckAdminUser())
            {
                List<Category> categories = _admin.GetCategories();
                return View(categories);
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }

        public IActionResult AddCategory()
        {
            if (CheckAdminUser())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    ParentId = null,
                    Name = viewModel.Name,
                    Icon = viewModel.Icon
                };
                _admin.InsertCategory(category);
                return Json(new { success = true, redirectUrl = "/Admin/Categories" });
            }
            return PartialView("AddCategory", viewModel);
        }

        public IActionResult EditCategory(int id)
        {
            if (CheckAdminUser())
            {
                Category category = _admin.GetCategory(id);
                CategoryViewModel viewModel = new CategoryViewModel()
                {
                    Name = category.Name,
                    Icon = category.Icon,
                    IsTopLevel = category.ParentId == null // این خط اضافه شده
                };
                return PartialView("EditCategory", viewModel);
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }

        [HttpPost]
        public IActionResult EditCategory(int id, CategoryViewModel viewModel)
        {
            // این خط را اضافه کنید تا ModelState را برای فیلد Icon پاک کنید.
            // این کار باعث می‌شود که حتی اگر فیلد Icon در فرم نباشد، ModelState.IsValid همچنان true برگرداند.
            ModelState.Remove("Icon");

            if (ModelState.IsValid)
            {
                Category existingCategory = _admin.GetCategory(id);
                if (existingCategory != null)
                {
                    string updatedIcon = viewModel.Icon;
                    if (string.IsNullOrEmpty(viewModel.Icon) && existingCategory.ParentId != null)
                    {
                        updatedIcon = existingCategory.Icon;
                    }

                    _admin.UpdateCategory(id, viewModel.Name, updatedIcon);
                }

                int? parentID = _admin.GetCategoryParentId(id);
                if (parentID != null)
                {
                    Category category1 = _admin.GetCategory((int)parentID);
                    if (category1.ParentId == null)
                    {
                        return Json(new { success = true, redirectUrl = "/Admin/SubCategories/" + parentID });
                    }
                    else
                    {
                        parentID = _admin.GetCategoryParentId((int)parentID);
                        Category category2 = _admin.GetCategory((int)parentID);
                        if (category2.ParentId == null)
                        {
                            return Json(new { success = true, redirectUrl = "/Admin/SubCategories/" + parentID });
                        }
                        else
                        {
                            parentID = _admin.GetCategoryParentId((int)parentID);
                            return Json(new { success = true, redirectUrl = "/Admin/SubCategories/" + parentID });

                        }
                    }
                }
                else
                {
                    return Json(new { success = true, redirectUrl = "/Admin/Categories/" + id });
                }
            }

            // اگر ModelState.IsValid برابر false بود، PartialView را به صورت یک رشته HTML برمی‌گردانیم
            return PartialView("EditCategory", viewModel);
        }

        public IActionResult DeleteCategory(int? id)
        {
            if (CheckAdminUser())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }

        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            if (ModelState.IsValid)
            {
                DeleteCascadeCategory(id);
                return Json(new { success = true, redirectUrl = "/Admin/Categories" });

            }
            return View();
        }

        private void DeleteCascadeCategory(int id)
        {
            if (CheckAdminUser())
            {

                List<Category> categories = _admin.GetCategoriesByParentId(id);
                if (categories.Count > 0)
                {
                    foreach (var item in categories)
                    {
                        DeleteCascadeCategory(item.Id);
                    }
                }
                _admin.DeleteCategory(id);

            }
            else
            {
                RedirectToAction("Dashboard", "Panel");
            }
        }

        public IActionResult SubCategories(int id)
        {
            if (CheckAdminUser())
            {
                List<Category> categories = _admin.GetSubCategories();
                ViewBag.MyId = id;
                return View(categories);
            }
            else
            {
                return RedirectToAction("Dashboard", "Panel");
            }
        }

        public IActionResult AddSubCategory(int id, int? parentId)
        {
            if (!CheckAdminUser())
                return RedirectToAction("Dashboard", "Panel");

            // اگر نیاز به ارسال ViewModel یا داده داری، اینجا بساز و ارسال کن
            return PartialView(new SubCategoriesViewModel());
        }

        [HttpPost]
        public IActionResult AddSubCategory(int id, SubCategoriesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    Icon = null,
                    ParentId = id,
                    Name = viewModel.Name
                };
                _admin.InsertCategory(category);

                int? parentID = _admin.GetCategoryParentId(id);
                if (parentID != null)
                {
                    Category category1 = _admin.GetCategory((int)parentID);
                    if (category1.ParentId == null)
                    {
                        return Json(new { success = true, redirectUrl = "/Admin/SubCategories/" + parentID });
                    }
                    else
                    {
                        parentID = _admin.GetCategoryParentId((int)parentID);
                        return Json(new { success = true, redirectUrl = "/Admin/SubCategories/" + parentID });
                    }
                }
                else
                {
                    return Json(new { success = true, redirectUrl = "/Admin/SubCategories/" + id });
                }
            }

            return PartialView("AddSubCategory", viewModel);
        }

        #endregion

        #region StoreCategories

        public IActionResult ShowStoreCategories()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                List<StoreCategory> storeCategories = _admin.GetStoreCategories();
                return View(storeCategories);
            }
        }


        public IActionResult EditStoreCategory(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var storeCategory = _admin.GetStoreCategory(id);
                UpdateStoreCategoryViewModel viewModel = new UpdateStoreCategoryViewModel()
                {
                    Description = storeCategory.Description,
                    Image = storeCategory.Image,
                    IsActive = storeCategory.IsActive

                };
                return PartialView("EditStoreCategory", viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditStoreCategory(int id, UpdateStoreCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _admin.UpdateStoreCategory(id, viewModel.IsActive, viewModel.Description);
                return Json(new { success = true, redirectUrl = "/Admin/ShowStoreCategories" });
            }
            return PartialView("EditStoreCategory", viewModel);

        }

        public IActionResult DeleteStoreCategory(int? id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var storeCategory = _admin.GetStoreCategory(id.Value);
                return PartialView("DeleteStoreCategory", storeCategory);
            }
        }

        [HttpPost]
        public IActionResult DeleteStoreCategory(int id)
        {
            _admin.DeleteStoreCategory(id);
            return Json(new { success = true, redirectUrl = "/Admin/ShowStoreCategories" });

        }

        #endregion

        #region Brands

        public IActionResult ShowBrands()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                List<Brand> brands = _admin.GetBrands();
                return View(brands);
            }
        }

        public IActionResult AddBrand()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
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
                        NotShow = viewModel.NotShow,
                        StoreId = null
                    };
                    _admin.AddBrand(brand);
                    return Json(new { success = true, redirectUrl = "/Admin/ShowBrands" });
                }
                else
                {
                    // ثبت برند
                    Brand brand = new Brand()
                    {
                        Image = null,
                        Name = viewModel.Name,
                        NotShow = viewModel.NotShow,
                        StoreId = null
                    };
                    _admin.AddBrand(brand);
                    return Json(new { success = true, redirectUrl = "/Admin/ShowBrands" });
                }
            }
            return PartialView("AddBrand", viewModel);
        }



        public IActionResult EditBrand(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Brand brand = _admin.GetBrand(id);
                AdminBrandViewModel viewModel = new AdminBrandViewModel()
                {
                    Name = brand.Name,
                    ImageName = brand.Image,
                    NotShow = brand.NotShow
                };
                // return View(brand);
                return PartialView("EditBrand", viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditBrand(AdminBrandViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                Brand brand = _admin.GetBrand(id);
                string imgName = null;

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                string[] allowedContentTypes = { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

                if (viewModel.Image != null)
                {
                    string extension = Path.GetExtension(viewModel.Image.FileName).ToLower();
                    string contentType = viewModel.Image.ContentType.ToLower();

                    if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(contentType))
                    {
                        ModelState.AddModelError("Image", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");

                        // مقدار ImageName را از دیتابیس مجدداً ست کن
                        viewModel.ImageName = brand.Image;

                        return PartialView("EditBrand", viewModel);
                    }

                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Brands/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }
                    _admin.UpdateBrand(id, viewModel.Name, imgName, viewModel.NotShow);
                    return Json(new { success = true, redirectUrl = "/Admin/ShowBrands" });
                }
                else
                {
                    _admin.UpdateBrand(id, viewModel.Name, brand.Image, viewModel.NotShow);
                    return Json(new { success = true, redirectUrl = "/Admin/ShowBrands" });
                }
            }

            // اگر اعتبارسنجی کلی ویو نامعتبر بود، مقدار ImageName را فراموش نکن تنظیم کنی
            var currentBrand = _admin.GetBrand(id);
            viewModel.ImageName = currentBrand.Image;

            return PartialView("EditBrand", viewModel);
        }


        public IActionResult DetailsBrand(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Brand brand = _admin.GetBrand(id);
                return View(brand);
            }
        }

        public IActionResult DeleteBrand(int? id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var Brand = _admin.GetBrand(id.Value);
                return PartialView("DeleteBrand", Brand);
            }
        }

        [HttpPost]
        public IActionResult DeleteBrand(int id)
        {
            _admin.DeleteBrand(id);
            return Json(new { success = true, redirectUrl = "/Admin/ShowBrands" });
        }

        #endregion

        #region Fields

        public IActionResult ShowFields()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                List<Field> fields = _admin.GetFields();
                return View(fields);
            }
        }


        public IActionResult AddField()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddField(FieldViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Field field = new Field()
                {
                    Name = viewModel.Name
                };

                _admin.AddField(field);
                return Json(new { success = true, redirectUrl = "/Admin/ShowFields" });
            }
            return PartialView("AddField", viewModel);
        }

        public IActionResult EditField(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Field field = _admin.GetField(id);
                FieldViewModel viewModel = new FieldViewModel()
                {
                    Name = field.Name,

                };
                return PartialView("EditField", viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditField(int id, FieldViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _admin.UpdateField(id, viewModel.Name);
                return Json(new { success = true, redirectUrl = "/Admin/ShowFields" });

            }
            return PartialView("EditField", viewModel);
        }

        public IActionResult DeleteField(int? id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var Field = _admin.GetField(id.Value);
                return PartialView("DeleteField", Field);
            }
        }

        [HttpPost]
        public IActionResult DeleteField(int id)
        {
            _admin.DeleteField(id);
            return Json(new { success = true, redirectUrl = "/Admin/ShowFields" });


        }

        public IActionResult FieldCategories(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                List<Field> fields = _admin.GetFields();
                ViewBag.CategoryId = id;
                return View(fields);
            }
        }

        [HttpPost]
        public IActionResult UpdateFieldCategory(int CategoryId, List<int> SelectedFields)
        {
            _admin.DeleteAll(CategoryId);

            if (SelectedFields != null && SelectedFields.Any())
            {
                foreach (var fieldId in SelectedFields)
                {
                    FieldCategory fieldCategory = new FieldCategory()
                    {
                        CategoryId = CategoryId,
                        FieldId = fieldId
                    };
                    _admin.AddFieldCategory(fieldCategory);
                }
            }

            // چون داخل مودال هست، می‌تونیم یک JSON برگردونیم یا Redirect کنیم
            return Json(new { success = true, redirectUrl = "/Admin/Categories" });
        }

        #endregion

        #region Products

        public IActionResult ShowAllProducts()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                List<Product> products = _admin.GetProducts();
                return View(products);
            }
        }


        public IActionResult ProductSeen(int id)
        {
            List<ProductSeen> productSeens = _admin.GetProductSeens(id);
            return View(productSeens);
        }

        public IActionResult DeleteProduct(int? id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var product = _admin.GetProduct(id.Value);
                return PartialView("DeleteProduct", product);
            }
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            _admin.RemoveProduct(id);
            return Json(new { success = true, redirectUrl = "/Admin/ShowAllProducts" });
        }


        //===========id = productId=================
        public IActionResult ProductDetails(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Product product = _admin.GetProduct(id);
                List<ProductGallery> productGalleries = _admin.GetProductGalleries(id);
                List<ProductField> productFields = _admin.GetProductFields(id);
                ProductAdminViewModel viewModel = new ProductAdminViewModel()
                {
                    FillProduct = product,
                    FillFields = productFields,
                    FillGalleries = productGalleries
                };

                return View(viewModel);
            }
        }

        #endregion

        #region Sliders

        public IActionResult ShowSliders()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                List<Slider> sliders = _admin.GetSliders();
                return View(sliders);
            }
        }

        public IActionResult AddSlider()
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddSlider(AdminSliderViewModel viewModel)
        {
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
                        return PartialView("AddSlider", viewModel);
                    }

                    // تولید نام جدید فایل
                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }
                    // ثبت اسلاید
                    Slider slider = new Slider()
                    {
                        Image = imgName,
                        Title = viewModel.Title,
                        NotShow = viewModel.NotShow,
                        Description = viewModel.Description,
                        OrderShow = viewModel.OrderShow
                    };
                    _admin.AddSlider(slider);
                    return Json(new { success = true, redirectUrl = "/Admin/ShowSliders" });
                    // return RedirectToAction("ShowSliders", "Admin");
                }
                else
                {
                    ModelState.AddModelError("Image", "فقط فرمت‌های تصویری مجاز هستند (jpg, jpeg, png, gif, bmp, webp)");
                    //  return PartialView("AddSlider", viewModel);
                    // return Json(new { success = true, redirectUrl = Url.Action("ShowBrands", "Admin") });
                }
            }
            return PartialView("AddSlider", viewModel);
        }


        public IActionResult EditSlider(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Slider slider = _admin.GetSlider(id);
                AdminSliderViewModel viewModel = new AdminSliderViewModel()
                {
                    Title = slider.Title,
                    Description = slider.Description,
                    OrderShow = slider.OrderShow,
                    NotShow = slider.NotShow,
                    ImageName = slider.Image
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditSlider(AdminSliderViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                Slider slider = _admin.GetSlider(id);
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
                        return PartialView("EditSlider", viewModel);
                    }

                    // تولید نام جدید فایل
                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }

                    string orderShow = viewModel.OrderShow.ToString();
                    orderShow = NumberConvertor.ToEnglishNumber(orderShow);

                    // ویرایش اسلاید
                    _admin.UpdateSlider(id, viewModel.Title, imgName, viewModel.Description, viewModel.NotShow, Int32.Parse(orderShow));
                    return Json(new { success = true, redirectUrl = "/Admin/ShowSliders" });
                }
                else
                {
                    string orderShow = viewModel.OrderShow.ToString();
                    orderShow = NumberConvertor.ToEnglishNumber(orderShow);
                    _admin.UpdateSlider(id, viewModel.Title, slider.Image, viewModel.Description, viewModel.NotShow, Int32.Parse(orderShow));
                    return Json(new { success = true, redirectUrl = "/Admin/ShowSliders" });
                }
            }
            return PartialView("EditSlider", viewModel);
        }

        public IActionResult DeleteSlider(int? id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var slider = _admin.GetSlider(id.Value);
                return PartialView("DeleteSlider", slider);
            }
        }

        [HttpPost]
        public IActionResult DeleteSlider(int id)
        {
            _admin.DeleteSlider(id);
            return Json(new { success = true, redirectUrl = "/Admin/ShowSliders" });
        }

        public IActionResult DetailsSlider(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Slider slider = _admin.GetSlider(id);
                return View(slider);
            }
        }

        #endregion

        #region Banners

        public IActionResult ShowBanners()
        {
            List<Banner> banners = _admin.GetBanners();
            return View(banners);
        }

        public IActionResult DetailsBanner(int id)
        {
            Banner banner = _admin.GetBanner(id);
            return View(banner);
        }

        public IActionResult EditBanner(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Banner banner = _admin.GetBanner(id);
                AdminBannerViewModel viewModel = new AdminBannerViewModel()
                {
                    Day = banner.Day,
                    Description = banner.Description,
                    ImageName = banner.DefaultImage,
                    Name = banner.Name,
                    Price = banner.Price,
                    Size = banner.Size
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult EditBanner(AdminBannerViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                Banner banner = _admin.GetBanner(id);
                string imgName = banner.DefaultImage;

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
                        return PartialView("EditBanner", viewModel);
                    }

                    // تولید نام جدید فایل
                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Ads/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }
                }
                // ویرایش اسلاید
                _admin.UpdateBanner(id, viewModel.Name, imgName, viewModel.Description, viewModel.Size, viewModel.Day, viewModel.Price);
                return Json(new { success = true, redirectUrl = "/Admin/ShowBanners" });

            }
            return PartialView("EditBanner", viewModel);
        }


        //برای اجاره بنر

        public IActionResult ShowBannerDetails(int id)
        {
            List<BannerDetails> bannerDetails = _admin.GetBannerDetails(id);
            ViewBag.BannerId = id;
            return View(bannerDetails);
        }


        public IActionResult AddBannerDetails(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddBannerDetails(AdminBannerDetailsViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                string imgName = "";

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
                        return PartialView("AddBannerDetails", viewModel);
                    }

                    // تولید نام جدید فایل
                    imgName = CodeGenerators.FileCode() + extension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Ads/", imgName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.Image.CopyTo(stream);
                    }
                }

                //1404/05/01
                string strToday = pc.GetYear(DateTime.Now).ToString("0000") + "/" +
                                pc.GetMonth(DateTime.Now).ToString("00") + "/" +
                                pc.GetDayOfMonth(DateTime.Now).ToString("00");

                Banner banner = _admin.GetBanner(id);
                DateTime dt = pc.ToDateTime(Int32.Parse(strToday.Substring(0, 4)), Int32.Parse(strToday.Substring(5, 2)), Int32.Parse(strToday.Substring(8, 2)), 0, 0, 0, 0);

                DateTime dtExpire = dt.AddDays(Convert.ToDouble(banner.Day));
                // ثبت بنر
                BannerDetails bannerDetails = new BannerDetails()
                {
                    BannerId = id,
                    Image = imgName,
                    Title = viewModel.Title,
                    IsExpire = false,
                    StartDate = pc.GetYear(DateTime.Now).ToString("0000") + "/" +
                                pc.GetMonth(DateTime.Now).ToString("00") + "/" +
                                pc.GetDayOfMonth(DateTime.Now).ToString("00"),
                    EndDate = pc.GetYear(dtExpire).ToString("0000") + "/" +
                              pc.GetMonth(dtExpire).ToString("00") + "/" +
                              pc.GetDayOfMonth(dtExpire).ToString("00"),
                    Url = viewModel.Url
                };
                _admin.AddBannerDetails(bannerDetails);
                return Json(new { success = true, redirectUrl = "/Admin/ShowBannerDetails/" + id });

            }
            return PartialView("AddBannerDetails", viewModel);
        }


        #endregion

        #region Coupons

        public IActionResult ShowAllCoupons()
        {
            List<Coupon> coupons = _admin.GetCoupons();
            return View(coupons);
        }

        public IActionResult ShowCouponsByStore(int id)
        {
            List<Coupon> coupons = _admin.GetCouponByStoreId(id);
            return View(coupons);
        }

        public IActionResult DetailsCoupon(int id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                Coupon coupon = _admin.GetCoupon(id);
                return View(coupon);
            }
        }

        public IActionResult DeleteCoupon(int? id)
        {
            if (!CheckAdminUser())
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                var coupon = _admin.GetCoupon(id.Value);
                return PartialView("DeleteCoupon", coupon);
            }
        }

        [HttpPost]
        public IActionResult DeleteCoupon(int id)
        {
            _admin.RemoveCoupon(id);
            return Json(new { success = true, redirectUrl = "/Admin/ShowAllCoupons" });
        }

        #endregion
    }
}