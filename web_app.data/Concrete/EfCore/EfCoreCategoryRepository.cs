
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_app.data.Abstract;
using web_app.entity;

namespace web_app.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int ProductId, int CategoryId)
        {
            using(var context = new ShopContext()){
                
                var cmd = "delete from ProductCategories where ProductId =@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd,ProductId,CategoryId);
            }
        }

        public Category GetByIdWithProducts(int categoryId)
        {
           using(var context = new ShopContext()){
               return context.Categories.Where(x=>x.CategoryId == categoryId).Include(x=>x.ProductCategories)
                        .ThenInclude(x=>x.Product).FirstOrDefault();
           }
        }

    }
}