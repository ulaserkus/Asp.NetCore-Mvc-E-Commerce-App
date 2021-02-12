using System;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

using web_app.data.Abstract;
using web_app.business.Abstract;
using App.webui.Models;

namespace App.webui.Controllers
{
    //localhost:5000/home
    public class HomeController : Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }
        //Index action
        public IActionResult Index()
        {
           
              //view models
            var productViewModel = new ProductListViewModel(){
                Products = _productService.GetHomePageProducts()
                
            };

            return View(productViewModel);
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {

            return View("MyView");
        }
        
        

    }
}