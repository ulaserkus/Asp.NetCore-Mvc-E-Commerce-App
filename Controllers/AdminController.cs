using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.webui.identity;
using App.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_app.business.Abstract;
using web_app.entity;

namespace App.webui.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductService _productservice;
        private ICategoryService _categoryservice;

        private RoleManager<IdentityRole> _roleManager;

        private UserManager<User> _userManager;
        public AdminController(IProductService ProductService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this._productservice = ProductService;
            this._categoryservice = categoryService;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var member in _userManager.Users)
            {

                var list = await _userManager.IsInRoleAsync(member, role.Name) ? members : nonmembers;

                list.Add(member);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var User = await _userManager.FindByIdAsync(userId);

                    if (User != null)
                    {
                        var result = await _userManager.AddToRoleAsync(User, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var User = await _userManager.FindByIdAsync(userId);

                    if (User != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(User, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }


            return Redirect("/admin/role/" + model.RoleId);
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));

                if (result.Succeeded)
                {

                    return RedirectToAction("RoleList");
                }
                else
                {

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return View();
        }
        public IActionResult ProductList()
        {

            return View(new ProductListViewModel()
            {
                Products = _productservice.GetAll()
            });
        }
        public IActionResult CategoryList()
        {

            return View(new CategoryListViewModel()
            {
                categories = _categoryservice.GetAll()
            });
        }
        [HttpGet]
        public IActionResult ProductCreate()
        {

            return View();

        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    ImageUrl = model.ImageUrl,
                    Description = model.Description,
                    Price = model.Price
                };
                if (_productservice.Create(entity))
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"{entity.Name} isimli ürün eklendi",
                        AlertType = "success"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                    return RedirectToAction("ProductList");
                }
                return View(model);


            }

            return View(model);

        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {

            return View();

        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url
                };
                _categoryservice.Create(entity);

                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} isimli kategori eklendi",
                    AlertType = "success"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);
                return RedirectToAction("CategoryList");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _productservice.GetProductByIdWithCategories((int)id);

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                Description = entity.Description,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(x => x.Category).ToList()


            };

            ViewBag.Categories = _categoryservice.GetAll();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var entity = _productservice.GetProductById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;

                entity.ProductId = model.ProductId;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if (file != null)
                {

                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }


                if (_productservice.Update(entity, categoryIds))
                {
                    CreateMessage("Kayıt Güncellendi", "warning");
                    return RedirectToAction("ProductList");
                }

                CreateMessage(_productservice.ErrorMessage, "danger");



            }
            ViewBag.Categories = _categoryservice.GetAll();
            return View(model);
        }
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _categoryservice.GetByIdWithProducts((int)id);

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()

            };

            return View(model);

        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryservice.GetProductById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;

                entity.Url = model.Url;


                _categoryservice.Update(entity);
                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} isimli kategori güncellendi",
                    AlertType = "warning"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("CategoryList");
            }

            return View(model);
        }

        public IActionResult DeleteProduct(int ProductId)
        {

            var entity = _productservice.GetProductById(ProductId);
            if (entity != null)
            {
                _productservice.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteCategory(int CategoryId)
        {

            var entity = _categoryservice.GetProductById(CategoryId);
            if (entity != null)
            {
                _categoryservice.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }


        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryservice.DeleteFromCategory(productId, categoryId);

            return RedirectToAction("CategoryList");
        }

        public IActionResult AccessDenied(int CategoryId)
        {
            return View();
        }

        public IActionResult UserList()
        {

            return View(_userManager.Users);
        }

        public async Task<IActionResult> UserEdit(string id)
        {

            var user =await _userManager.FindByIdAsync(id);
            if(user!=null){
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var Roles = _roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = Roles;
                
                return View (new UserDetailsModel(){
                    UserId=user.Id,
                    UserName= user.UserName,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    Email=user.Email,
                    EmailConfirmed=user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });

            }

            return Redirect("~/admin/user/list");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model,string[] selectedRoles)
        {

            if(ModelState.IsValid){

                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null){

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded){
 
                        var userRoles =await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string []{};
                        
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>());
                         return Redirect("~/admin/user/list");
                    }
                }
                return Redirect("~/admin/user/list");
            }

           return View(model);
        }


        public void CreateMessage(string message, string AlertType)
        {

            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = AlertType

            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}


