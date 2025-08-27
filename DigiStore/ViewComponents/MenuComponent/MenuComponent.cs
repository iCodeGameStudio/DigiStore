using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.ViewComponents.MenuComponent
{
    public class MenuComponent:ViewComponent
    {
        private ITemp _temp;

        public MenuComponent(ITemp temp)
        {
            _temp = temp;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("MenuView",_temp.GetCategories()));
        }
    }
}
