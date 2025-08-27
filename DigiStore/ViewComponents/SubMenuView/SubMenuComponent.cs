using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;


namespace DigiStore.ViewComponents.MenuComponent
{

    public class SubMenuComponent : ViewComponent
    {
        private ITemp _temp;

        public SubMenuComponent(ITemp temp)
        {
            _temp = temp;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return await Task.FromResult((IViewComponentResult)View("SubMenuView", _temp.GetCategoryById(id)));
        }
    }
}
