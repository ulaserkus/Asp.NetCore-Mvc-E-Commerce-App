using System;
using System.Collections.Generic;
using web_app.entity;

namespace web_app.business.Abstract
{
    public interface IProductService:IValidator<Product>
    {


        Product GetProductById(int id);

        List<Product> GetAll();

        List<Product> GetProductsByCategory(string name,int page,int pageSize);

        Product GetProductDetails(string url);
        bool Create(Product entity);

        void Update(Product entity);

        void Delete(Product entity);
        int GetCountByCategory(string category);
         List<Product>GetSearchResult(string searchString);

        List<Product>GetHomePageProducts();
        Product GetProductByIdWithCategories(int id);
        bool Update(Product entity, int[] categoryIds);
    }
}
