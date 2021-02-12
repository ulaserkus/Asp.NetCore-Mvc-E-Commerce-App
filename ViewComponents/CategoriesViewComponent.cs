using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using web_app.business.Abstract;

namespace App.webui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private ICategoryService _categoryservice;

        public CategoriesViewComponent(ICategoryService categoryservice)
        {
            this._categoryservice= categoryservice;
        }
        public IViewComponentResult Invoke()
        {
           
            if(RouteData.Values["category"]!=null)
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            // return View(CategoryRepository.Categories);
            return View(_categoryservice.GetAll());
        }
    }
}