using Microsoft.AspNetCore.Mvc;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;


namespace DigiStore.ViewComponents.MenuByIdComponent
{
    public class MenuByIdComponent:ViewComponent
    {
        private ITemp _temp;

        public MenuByIdComponent(ITemp temp)
        {
            _temp = temp;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return await Task.FromResult((IViewComponentResult)View("MenuByIdView", _temp.GetCategoryById(id)));
        }
    }
}
