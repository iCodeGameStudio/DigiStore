using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;



namespace DigiStore.Core.Interfaces
{
    public interface ITemp
    {
        #region Category
        List<Category> GetCategories();

        List<Category> GetCategoryById(int id);

        Category GetCategory(int id);

        #endregion

        #region Slider        
        List<Slider> GetSliders();

        #endregion

        #region Banner
        List<Banner> GetBanners();

        bool CheckBannerImg(int id);

        BannerDetails GetBannerDetails(int id);

        void UpdateBannerExpire(int id);

        List<BannerDetails> GetBannerDetailsNoExpire();

        #endregion

        #region Products

        List<Product> GetProducts(int id);

        List<Product> GetProductsOffers();

        #endregion

        #region Store

        List<Store> GetStores(int id);

        #endregion
    }
}
