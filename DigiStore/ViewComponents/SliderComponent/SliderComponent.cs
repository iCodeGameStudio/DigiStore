using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;


namespace DigiStore.ViewComponents.SliderComponent
{
    public class SliderComponent:ViewComponent
    {
        private ITemp _temp;

        public SliderComponent(ITemp temp)
        {
            _temp = temp;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("SliderView", _temp.GetSliders()));
        }
    }
}
