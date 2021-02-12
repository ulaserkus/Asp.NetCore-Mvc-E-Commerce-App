using System;
using System.Collections.Generic;
using web_app.entity;

namespace web_app.data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    { 
        Product GetProductDetails(string url);
        List<Product> GetPopularProducts();
        List<Product>GetSearchResult(string searchString);
        List <Product>GetHomePageProducts();
       List<Product> GetProductByCategory(string name,int page,int pageSize);

        int GetCountByCategory(string category);

       Product GetProductByIdWithCategories(int id);

        void Update(Product entity, int[] categoryIds);
    }
}
