using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Core.Interfaces;
using DigiStore.DataAccessLayer.Entities;
using DigiStore.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Core.Services
{
    public class StoreService : IStore
    {
        private DatabaseContext _context;

        public StoreService(DatabaseContext context)
        {
            _context = context;
        }

        public void AddBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void AddStoreCategory(StoreCategory storeCategory)
        {
            _context.StoreCategories.Add(storeCategory);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public void DeleteStoreCategory(int id)
        {
            StoreCategory storeCategory = _context.StoreCategories.Find(id);
            _context.StoreCategories.Remove(storeCategory);
            _context.SaveChanges();
        }

        public bool ExistsBrand(string name)
        {
            return _context.Brands.Any(b => b.Name == name);
        }

        public List<Brand> GetBrands(int id)
        {
            return _context.Brands.Where(b => b.StoreId == id).OrderByDescending(b => b.Id).ToList();
        }

        public List<Brand> AllBrands()
        {
            return _context.Brands.OrderBy(b => b.Name).ToList();
        }

        public List<Category> GetCategoriesByNullParent()
        {
            return _context.Categories.Where(c => c.ParentId == null).OrderBy(c => c.Name).ToList();
        }

        public string GetLogoStore(string username)
        {
            return _context.Stores.Where(s => s.User.Mobile == username).Select(s => s.logo).FirstOrDefault();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Include(p => p.Brand).Include(p => p.Category).FirstOrDefault(p => p.Id == id);

        }

        public List<Product> GetProducts(int id)
        {
            return _context.Products.Where(p => p.StoreId == id).OrderByDescending(p => p.Id).ToList();
        }

        public Store GetStore(int id)
        {
            return _context.Stores.Include(s => s.User).FirstOrDefault(s => s.UserId == id);
        }

        public List<StoreCategory> GetStoreCategoriesByStoreId(int id)
        {
            return _context.StoreCategories.Include(s => s.Category).Where(s => s.UserId == id).OrderByDescending(s => s.Id).ToList();
        }

        public StoreCategory GetStoreCategory(int id)
        {
            return _context.StoreCategories.Find(id);
        }

        public void UpdateProduct(int id, int brandId, int CategoryId, string name, string image, int price, int deletePrice, int exists, bool notshow, string description)
        {
            Product product = _context.Products.Find(id);
            product.BrandId = brandId;
            product.CategoryId = CategoryId;
            product.Name = name;
            product.Image = image;
            product.Price = price;
            product.DeletePrice = deletePrice;
            product.Exist = exists;
            product.NotShow = notshow;
            product.Description = description;
            _context.SaveChanges();
        }

        public bool UpdateStore(int id, string name, string tel, string address, string description, string logo)
        {
            Store store = _context.Stores.Find(id);

            if (store != null)
            {
                store.Name = name;
                store.Tel = tel;
                store.Address = address;
                store.Description = description;
                store.logo = logo;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateStoreCategory(int id, int categoryid, string image)
        {
            StoreCategory storeCategory = _context.StoreCategories.Find(id);
            storeCategory.CategoryId = categoryid;
            storeCategory.Image = image;
            storeCategory.IsActive = false;
            _context.SaveChanges();
        }

        public List<Category> GetCategories(int id)
        {
               return _context.Categories.Where(c => c.ParentId == id).ToList();
        }


        // گرفتن همه زیر دسته‌ها (ریکرسیو)
        public List<Category> GetAllSubCategories(int categoryId)
        {
            return _context.Categories.Where(c => c.ParentId == categoryId).ToList().SelectMany(c => new[] { c }.Concat(GetAllSubCategories(c.Id))).ToList();
        }




        public void AddGallery(ProductGallery productGallery)
        {
            _context.ProductGalleries.Add(productGallery);
            _context.SaveChanges();
        }

        public void DeleteGallery(int id)
        {
            ProductGallery productGallery = _context.ProductGalleries.Find(id);
            _context.ProductGalleries.Remove(productGallery);
            _context.SaveChanges();
        }

        public ProductGallery GetProductGallery(int id)
        {
            return _context.ProductGalleries.Find(id);
        }

        public List<ProductGallery> GetProductGalleries(int id)
        {
            return _context.ProductGalleries.Where(pg => pg.ProductId == id).ToList();
        }

        public void AddProductField(ProductField productField)
        {
            _context.ProductFields.Add(productField);
            _context.SaveChanges();
        }

        public void DeleteAllProductFields(int id)
        {
            List<ProductField> productFields = _context.ProductFields.Where(pf => pf.ProductId == id).ToList();
            _context.ProductFields.RemoveRange(productFields);
            _context.SaveChanges();
        }

        public List<ProductField> GetProductFields(int id)
        {
            return _context.ProductFields.Where(pf => pf.ProductId == id).ToList();
        }

        public ProductField GetProductField(int id, int pid)
        {
            return _context.ProductFields.Include(pf => pf.Field).FirstOrDefault(pf => pf.ProductId == pid && pf.FieldId == id);
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Include(c => c.Parent).FirstOrDefault(c => c.Id == id);
        }

        public List<FieldCategory> GetFieldCategories(int id)
        {
            return _context.FieldCategories.Include(fc => fc.Field).Where(fc => fc.CategoryId == id).ToList();
        }

        public int GetProductSeen(int id)
        {
            return _context.ProductSeens.Where(s => s.ProductId == id).Count();
        }

        public List<StoreCategory> GetStoreCategoriesActivated(int userId)
        {
            return _context.StoreCategories.Where(s => s.UserId == userId && s.IsActive == true).ToList();
        }

        public List<Category> GetAllActivatedCategoriesForUser(int userId)
        {
            return GetStoreCategoriesActivated(userId).Select(s => s.CategoryId).SelectMany(cid => GetAllSubCategories(cid)).Where(c => c.ParentId != null).ToList();
        }

        public string GetNameStore(int UserId)
        {
            return _context.Stores.Where(s => s.UserId == UserId).FirstOrDefault().Name;
        }

        public bool CheckActivateStore(int UserId)
        {
            return _context.Stores.Where(s => s.UserId == UserId && s.MailActivate == true && s.MobileActivate == true).Any();
        }

        public bool ExistsCouponCode(string code)
        {
            return _context.Coupons.Any(c => c.Code == code);
        }

        public void AddCoupon(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
            _context.SaveChanges();
        }

        public void UpdateCoupon(int id, string name, string code, bool expire, string description, string start, string end, int percent, int price)
        {
            Coupon coupon = _context.Coupons.Find(id);
            if(!_context.Coupons.Any(c => c.Code == code && c.Id != id))
            {
                coupon.Code = code;
            }
            coupon.Name = name;           
            coupon.IsExpire = expire;
            coupon.Description = description;
            coupon.StartDate = start;
            coupon.EndDate = end;
            coupon.Percent = percent;
            coupon.Price = price;
            _context.SaveChanges();

        }

        public void RemoveCoupon(int id)
        {
            Coupon coupon = _context.Coupons.Find(id);
            _context.Coupons.Remove(coupon);
            _context.SaveChanges();
        }

        public List<Coupon> GetCoupons(int id)
        {
            return _context.Coupons.Where(c => c.StoreId == id).ToList();
        }

        public Coupon GetCoupon(int id)
        {
            return _context.Coupons.Find(id);
        }

        public void UpdateBankCard(int id, string card)
        {
            Store store = _context.Stores.Find(id);
            store.BankCard = card;
            _context.SaveChanges();
        }

        public async Task SetCoverImage(int galleryId, int productId)
        {
            // استفاده از AsNoTracking برای خواندن اولیه می‌تواند بهینه‌تر باشد،
            // اما برای سادگی فعلاً آن را حذف می‌کنیم.
            var galleries = await _context.ProductGalleries
                .Where(g => g.ProductId == productId)
                .ToListAsync();

            foreach (var gallery in galleries)
            {
                // چه کاور باشد چه نباشد، آن را false کن
                gallery.Cover = false;
            }

            var selectedGallery = galleries.FirstOrDefault(g => g.Id == galleryId);
            if (selectedGallery != null)
            {
                selectedGallery.Cover = true;
            }
            else
            {
                // این حالت نباید اتفاق بیفتد اگر galleryId معتبر باشد
                // اما برای اطمینان می‌توانید آن را لاگ کنید.
            }

            await _context.SaveChangesAsync();
        }
    }
}
