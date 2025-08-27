using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.Core.Interfaces
{
    public interface IStore
    {
        Store GetStore(int id);

        bool UpdateStore(int id, string name, string tel, string address, string description, string logo);

        List<Category> GetCategoriesByNullParent();

        string GetLogoStore(string username);

        string GetNameStore(int UserId);

        bool CheckActivateStore(int UserId);

        void UpdateBankCard(int id, string card);
        
        #region StoreCategory

        void AddStoreCategory(StoreCategory storeCategory);
        void UpdateStoreCategory(int id, int categoryid, string image);
        void DeleteStoreCategory(int id);
        List<StoreCategory> GetStoreCategoriesByStoreId(int id);
        StoreCategory GetStoreCategory(int id);
        // همه StoreCategory های فعال فروشنده
        List<StoreCategory> GetStoreCategoriesActivated(int userId);

        // همه زیر دسته‌های یک دسته (بازگشتی)
        public List<Category> GetAllSubCategories(int categoryId);

        // همه زیر دسته‌های فعال فروشنده (تک متد برای کنترلر)
        List<Category> GetAllActivatedCategoriesForUser(int userId);

        #endregion

        #region Brand

        void AddBrand(Brand brand);
        List<Brand> GetBrands(int id);
        bool ExistsBrand(string name);

        List<Brand> AllBrands();

        #endregion

        #region Product

        void AddProduct(Product product);

        void DeleteProduct(int id);

        void UpdateProduct(int id, int brandId, int CategoryId, string name, string image, int price, int deletePrice, int exists, bool notshow, string description);

        Product GetProduct(int id);

        List<Product> GetProducts(int id);

        List<Category> GetCategories(int id);

        Category GetCategory(int id);

        int GetProductSeen(int id);

        #endregion

        #region Gallery

        void AddGallery(ProductGallery productGallery);
        void DeleteGallery(int id);

        ProductGallery GetProductGallery(int id);

        List<ProductGallery> GetProductGalleries(int id);

        Task SetCoverImage(int galleryId, int productId);
        #endregion

        #region ProductField

        void AddProductField(ProductField productField);

        void DeleteAllProductFields(int id);

        List<ProductField> GetProductFields(int id);

        ProductField GetProductField(int id,int pid);

        List<FieldCategory> GetFieldCategories(int id);

        #endregion

        #region Coupon

        bool ExistsCouponCode(string code);

        void AddCoupon(Coupon coupon);

        void UpdateCoupon(int id, string name, string code, bool expire, string description, string start, string end, int percent, int price);

        void RemoveCoupon(int id);

        List<Coupon> GetCoupons(int id);

        Coupon GetCoupon(int id);

        #endregion
    }
}
