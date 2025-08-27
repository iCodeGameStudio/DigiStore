using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.ViewComponents.ProductOffers
{
    public class ProductOfferComponent:ViewComponent
    {
        private ITemp _temp;

        public ProductOfferComponent(ITemp temp)
        {
            _temp = temp;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("ProductOfferView", _temp.GetProductsOffers()));
        }
    }
}
