using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.ViewComponents.BannerComponent
{
    public class BannerComponent:ViewComponent
    {
        private ITemp _temp;

        public BannerComponent(ITemp temp)
        {
            _temp = temp;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("BannerView", _temp.GetBanners()));
        }
    }
}
