using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_app.data.Abstract;
using web_app.entity;

namespace web_app.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {

                var products = context.Products.Where(x => x.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                               .Include(i => i.ProductCategories)
                               .ThenInclude(i => i.Category)
                               .Where(i => i.ProductCategories.Any(a => a.Category.Url == category));


                }
                return products.Count();

            }


        }

        public List<Product> GetHomePageProducts()
        {

            using (var context = new ShopContext())
            {

                return context.Products.Where(x => x.IsHome == true && x.IsApproved == true).ToList();
            }
        }

        public List<Product> GetPopularProducts()
        {
            using (var context = new ShopContext())
            {

                return context.Products.ToList();
            }
        }

        public List<Product> GetProductByCategory(string name, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {

                var products = context.Products.Where(x => x.IsApproved)
                .AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                               .Include(i => i.ProductCategories)
                               .ThenInclude(i => i.Category)
                               .Where(i => i.ProductCategories.Any(a => a.Category.Url == name));


                }
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            }
        }

        public Product GetProductByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(x => x.ProductId == id).Include(x => x.ProductCategories)
                 .ThenInclude(x => x.Category).FirstOrDefault();
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(i => i.Url == url)
                             .Include(p => p.ProductCategories)
                             .ThenInclude(i => i.Category)
                             .FirstOrDefault();
            }
        }

        public List<Product> GetSearchResult(string searchString)
        {

            using (var context = new ShopContext())
            {


                var products = context.Products
                                        .Where(x => x.IsApproved && x.Name.ToLower().Contains(searchString.ToLower()) || (x.Description.ToLower().Contains(searchString.ToLower())))
                                        .ToList();

                return products;
            }

        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                .Include(i => i.ProductCategories)
                .FirstOrDefault(i => i.ProductId == entity.ProductId);

                if (product != null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.ImageUrl = entity.ImageUrl;
                    product.IsHome = entity.IsHome;
                    product.IsApproved = entity.IsApproved;
                

                    product.ProductCategories = categoryIds.Select(catId =>new ProductCategory(){
                       ProductId = entity.ProductId,
                       CategoryId =catId
                    }).ToList();
                    
                    context.SaveChanges();
                }
            }

        }
    }
}
